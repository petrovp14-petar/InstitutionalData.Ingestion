using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using InstitutionalData.Ingestion.Models;
using InstitutionalData.Ingestion.Repositories;
using InstitutionalData.Ingestion.Services;

namespace InstitutionalData.Ingestion.Tests;

public class DataIngestionServiceTests
{
    private readonly Mock<IUserRepository> _mockRepo = new();
    private readonly Mock<ILogger<DataIngestionService>> _mockLogger = new();
    private readonly DataIngestionService _service;

    public DataIngestionServiceTests()
    {
        // Injecting our mocked interface components into the .NET 10 Primary Constructor
        _service = new DataIngestionService(_mockRepo.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ProcessUserIngestionAsync_ShouldCreateNewUser_WhenUserDoesNotExist()
    {
        // Arrange - Setup a fresh mock incoming data entity
        var newUser = new UserIngestionDto
        {
            SystemId = "999999999",
            Email = "test@buffalo.edu",
            FirstName = "Jane",
            LastName = "Doe",
            IsActive = true
        };

        _mockRepo.Setup(r => r.ExistsAsync(newUser.SystemId)).ReturnsAsync(false);
        _mockRepo.Setup(r => r.CreateAsync(newUser)).ReturnsAsync(true);

        // Act - Run the ingestion pipeline engine
        var result = await _service.ProcessUserIngestionAsync(newUser);

        // Assert - Confirm structural business rule expectations passed safely
        Assert.True(result);
        _mockRepo.Verify(r => r.CreateAsync(newUser), Times.Once);
        _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<UserIngestionDto>()), Times.Never);
    }
}
