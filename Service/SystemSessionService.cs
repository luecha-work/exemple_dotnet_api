using Entities;
using IRepository;
using IService;
using Microsoft.AspNetCore.Http;
using Shared.DTOs;
using Shared.Enum;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SystemSessionService : ISystemSessionService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SystemSessionService(
            IRepositoryManager repositoryManager,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _repositoryManager = repositoryManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SystemSessionExpiredAsync(SystemSession systemSession)
        {
            systemSession.SessionStatus = EnumHelper.GetEnumValue(SessionStatusEnum.Expired);
            systemSession.UpdatedAt = DateTime.UtcNow;

            await _repositoryManager.SystemSessionRepository.UpdateAsync(systemSession);
        }

        public async Task BlockSystemSessionAsync(SystemSession systemSession)
        {
            systemSession.SessionStatus = EnumHelper.GetEnumValue(SessionStatusEnum.Blocked);
            systemSession.UpdatedAt = DateTime.UtcNow;

            await _repositoryManager.SystemSessionRepository.UpdateAsync(systemSession);
        }

        public async Task<bool> CheckSystemSessionStatusAsync(Guid sessionId, int accountId, string reqIpAddress)
        {
            var session =
                await _repositoryManager.SystemSessionRepository.GetBySessionIdAsync(sessionId);

            if (session == null) return false;

            if (session.AccountId != accountId || session.LoginIp != reqIpAddress)
            {
                //TODO: Block Axons Cms Session
                await BlockSystemSessionAsync(session);

                return false;
            }

            if (session.SessionStatus != EnumHelper.GetEnumValue(SessionStatusEnum.Active)) return false;

            if (DateTime.UtcNow > session.ExpirationTime)
            {
                //TODO: Axons Cms Session Expired
                await SystemSessionExpiredAsync(session);

                return false;
            }

            return true;
        }

        public async Task ClearSessionAsync()
        {
            string authorizationHeader = GetAuthorizationHeader();

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                throw new Exception("Clear session notfound authorization token.");
            }

            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            string sessionId = JWTHelper.GetSessionIdFromToken(token);

            var session =
               await _repositoryManager.SystemSessionRepository.GetBySessionIdAsync(Guid.Parse(sessionId));

            if (session != null)
            {
                _repositoryManager.SystemSessionRepository.Delete(session);
                _repositoryManager.Commit();
            }
        }

        public async Task<SystemSession> CreateSystemSessionAsync(Account account, BaseAuthenticationDto clientDetail)
        {
            var ipAddr = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
            var dateNow = DateTime.UtcNow;
            var dateExpiration = dateNow.AddHours(24);

            var oldSystemSession = (await _repositoryManager.SystemSessionRepository.FindByConditionAsync(session =>
                    session.AccountId == account.Id && session.LoginIp == ipAddr)).FirstOrDefault();

            if (oldSystemSession != null)
            {
                await DeleteSystemSessionAsync(oldSystemSession.Id);
            }

            var sessionForCreate = new SystemSession()
            {
                AccountId = account.Id,
                Browser = clientDetail.Browser,
                Os = clientDetail.Os,
                Platform = clientDetail.PlatForm,
                LoginIp = ipAddr ?? "",
                SessionStatus = EnumHelper.GetEnumValue(SessionStatusEnum.Active),
                IssuedTime = dateNow,
                ExpirationTime = dateExpiration,
                LoginAt = dateNow,
                CreatedAt = dateNow,
                UpdatedAt = dateNow,
            };

            _repositoryManager.SystemSessionRepository.Create(sessionForCreate);
            _repositoryManager.Commit();

            return sessionForCreate;
        }

        public async Task DeleteSystemSessionAsync(Guid sessionId)
        {
            var checkSession = await _repositoryManager.SystemSessionRepository.GetBySessionIdAsync(sessionId);

            if (checkSession != null)
            {
                _repositoryManager.SystemSessionRepository.Delete(checkSession);
                _repositoryManager.Commit();
            }
        }

        public async Task<SystemSession> GetSystemSessionByIdAsync(Guid sessionId)
        {
            var session = await _repositoryManager.SystemSessionRepository.GetBySessionIdAsync(sessionId);
            if (session == null)
            {
                throw new Exception("Session not found.");
            }

            return session;
        }

        // public void UpdateSystemSession(SystemSession systemSession)
        // {
        //     _repositoryManager.SystemSessionRepository.Update(systemSession);
        //     _repositoryManager.Commit();
        // }

        private string GetAuthorizationHeader()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Request.Headers.Authorization.Count > 0)
            {
                return httpContext.Request.Headers.Authorization.ToString();
            }
            return string.Empty;
        }
    }
}
