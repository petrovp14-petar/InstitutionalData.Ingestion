using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using InstitutionalData.Ingestion.Models;
using InstitutionalData.Ingestion.Repositories;

namespace InstitutionalData.Ingestion.Services;

public class DataIngestionService(IUserRepository userRepository, ILogger<DataIngestionService> logger)
{
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    private readonly ILogger<DataIngestionService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<bool> ProcessUserIngestionAsync(UserIngestionDto userDto)
    {
        if (userDto == null)
        {
            _logger.LogWarning("Ingestion failed: Received null user record.");
            return false;
        }

        _logger.LogInformation("Processing ingestion for System ID: {SystemId}", userDto.SystemId);

        try
        {
            bool exists = await _userRepository.ExistsAsync(userDto.SystemId);

            if (exists)
            {
                _logger.LogInformation("System ID {SystemId} already exists. Executing record update.", userDto.SystemId);
                return await _userRepository.UpdateAsync(userDto);
            }

            _logger.LogInformation("System ID {SystemId} is new. Executing insert transaction.", userDto.SystemId);
            return await _userRepository.CreateAsync(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred while processing System ID: {SystemId}", userDto.SystemId);
            throw;
        }
    }
}
