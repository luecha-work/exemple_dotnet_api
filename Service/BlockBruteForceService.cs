using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entities;
using IRepository;
using IService;
using Shared.Enum;
using Shared.Utils;

namespace Service
{
    public class BlockBruteForceService : IBlockBruteForceService
    {

        private readonly IRepositoryManager _repositoryManager;
        private BlockBruteForce? _bruteForce;

        public BlockBruteForceService(
            IRepositoryManager repositoryManager
        )
        {
            _repositoryManager = repositoryManager;
        }

        public async Task<BlockBruteForce?> BlockBruteForceManagementAsync(string email)
        {
            _bruteForce = (await _repositoryManager.BlockBruteForceRepository.FindByConditionAsync(force => force.Email == email)).FirstOrDefault();

            if (_bruteForce == null)
            {
                // CreateBlockBruteForce(email);
                BlockBruteForce newBruteForce = new BlockBruteForce()
                {
                    Count = 1,
                    Email = email,
                    Status = EnumHelper.GetEnumValue(BlockForceStatusEnum.UnLock),
                    LockedTime = null,
                    UnLockTime = null,
                    CreatedBy = "system",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    UpdatedBy = null
                };

                _repositoryManager.BlockBruteForceRepository.Create(newBruteForce);
                _repositoryManager.Commit();
            }
            else if (_bruteForce.Count >= 4)
            {
                //TODO: Add UnLockTime in Block Bruteforce
                var dateNow = DateTime.UtcNow;
                var unLockTime = dateNow.AddMinutes(15);

                _bruteForce.Count += 1;
                _bruteForce.Status = EnumHelper.GetEnumValue(BlockForceStatusEnum.Locked);
                _bruteForce.LockedTime = dateNow;
                _bruteForce.UnLockTime = unLockTime;
                _bruteForce.UpdatedAt = DateTime.UtcNow;

                _repositoryManager.BlockBruteForceRepository.Update(_bruteForce);
                _repositoryManager.Commit();
            }
            else
            {
                _bruteForce.Count += 1;
                _repositoryManager.BlockBruteForceRepository.Update(_bruteForce);
                _repositoryManager.Commit();
            }

            return _bruteForce;
        }

        public async Task<bool> CheckBlockForceStatusAsync(string email)
        {
            var blockForce = (await _repositoryManager.BlockBruteForceRepository.FindByConditionAsync(force => force.Email == email)).FirstOrDefault();

            if (blockForce == null)
            {
                return true;
            }

            if (
                blockForce.Status == EnumHelper.GetEnumValue(BlockForceStatusEnum.UnLock)
                && blockForce.Count < 5
            )
            {
                return true;
            }

            if (
                blockForce.Status == EnumHelper.GetEnumValue(BlockForceStatusEnum.Locked)
                && DateTime.UtcNow > blockForce.UnLockTime
            )
            {
               await UnLockBlockBruteForceAsync(blockForce);

                // await _repositoryManager.AccountRepository.UnLockAccountAsync(email);

                return true;
            }

            return false;
        }

        public async Task<BlockBruteForce?> CreateBlockBruteForceAsync(string email)
        {
            BlockBruteForce newBruteForce = new BlockBruteForce()
            {
                Count = 1,
                Email = email,
                Status = EnumHelper.GetEnumValue(BlockForceStatusEnum.UnLock),
                LockedTime = null,
                UnLockTime = null,
                CreatedBy = "system",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                UpdatedBy = null
            };


            await _repositoryManager.BlockBruteForceRepository.CreateAsync(newBruteForce);

            return _bruteForce;
        }

        public async Task DeleteBlockBruteForceAsync(Guid blockForceId)
        {
            var checkBruteForce = await _repositoryManager.BlockBruteForceRepository.GetBlockBruteForceByIdAsync(blockForceId);

            if (checkBruteForce != null)
            {
                _repositoryManager.BlockBruteForceRepository.Delete(checkBruteForce);
                _repositoryManager.Commit();
            }
        }

        private async Task UnLockBlockBruteForceAsync(BlockBruteForce blockForce)
        {
            if (blockForce.Status == EnumHelper.GetEnumValue(BlockForceStatusEnum.Locked))
            {
                blockForce.Status = EnumHelper.GetEnumValue(BlockForceStatusEnum.UnLock);
                blockForce.UpdatedAt = DateTime.UtcNow;
                blockForce.Count = 0;
                blockForce.LockedTime = null;
                blockForce.UnLockTime = null;

               await _repositoryManager.BlockBruteForceRepository.UpdateAsync(blockForce);
            }
        }
    }
}
