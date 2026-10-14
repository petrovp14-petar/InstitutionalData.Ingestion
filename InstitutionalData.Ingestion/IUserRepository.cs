using System.Threading.Tasks;
using InstitutionalData.Ingestion.Models;

namespace InstitutionalData.Ingestion.Repositories;

public interface IUserRepository
{
    Task<bool> ExistsAsync(string systemId);
    Task<bool> CreateAsync(UserIngestionDto user);
    Task<bool> UpdateAsync(UserIngestionDto user);
}
