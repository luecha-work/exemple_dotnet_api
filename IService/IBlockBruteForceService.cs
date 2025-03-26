using Entities;

namespace IService
{
    public interface IBlockBruteForceService
    {
        Task<BlockBruteForce> CreateBlockBruteForceAsync(string email);
        // void UpdateBlockBruteForce(BlockBruteForce blockForce);
        Task DeleteBlockBruteForceAsync(Guid blockForceId);
        Task<BlockBruteForce?> BlockBruteForceManagementAsync(string email);
        Task<bool> CheckBlockForceStatusAsync(string email);
    }
}
