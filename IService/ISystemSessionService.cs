using Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService
{
    public interface ISystemSessionService
    {
        Task<SystemSession> GetSystemSessionByIdAsync(Guid sessionId);
        Task<SystemSession> CreateSystemSessionAsync(
            Account account,
            BaseAuthenticationDto clientDetail
        );
        // void UpdateSystemSession(SystemSession SystemSession);
        Task DeleteSystemSessionAsync(Guid sessionId);
        Task<bool> CheckSystemSessionStatusAsync(
            Guid sessionId,
            int accountId,
            string reqIpAddress
        );
        Task BlockSystemSessionAsync(SystemSession systemSession);
        Task SystemSessionExpiredAsync(SystemSession systemSession);
        Task ClearSessionAsync();
    }
}
