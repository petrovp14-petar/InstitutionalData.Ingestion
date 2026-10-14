using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using InstitutionalData.Ingestion.Models;

namespace InstitutionalData.Ingestion.Repositories;

public class UserRepository(IDbConnection dbConnection) : IUserRepository
{
    private readonly IDbConnection _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

    public async Task<bool> ExistsAsync(string systemId)
    {
        const string sql = "SELECT COUNT(1) FROM dbo.ApplicationUsers WHERE SystemId = @SystemId;";
        int count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { SystemId = systemId });
        return count > 0;
    }

    public async Task<bool> CreateAsync(UserIngestionDto user)
    {
        const string sql = @"
            INSERT INTO dbo.ApplicationUsers (SystemId, Email, FirstName, LastName, IsActive, CreatedAt)
            VALUES (@SystemId, @Email, @FirstName, @LastName, @IsActive, GETUTCDATE());";

        int rowsAffected = await _dbConnection.ExecuteAsync(sql, user);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(UserIngestionDto user)
    {
        const string sql = @"
            UPDATE dbo.ApplicationUsers 
            SET Email = @Email, 
                FirstName = @FirstName, 
                LastName = @LastName, 
                IsActive = @IsActive, 
                UpdatedAt = GETUTCDATE()
            WHERE SystemId = @SystemId;";

        int rowsAffected = await _dbConnection.ExecuteAsync(sql, user);
        return rowsAffected > 0;
    }
}
