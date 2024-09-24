using ApiApplication.Api.Controllers;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Domain.Exceptions;
using ApiApplication.Domain.Services;
using ApiApplication.Domain.Services.Interfaces;
using ApiApplication.GrpcServices.GrpcServices;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;


namespace ApiApplication.Tests;

public class AuditoriumServicesTests
{
    private readonly Mock<IAuditoriumsRepository> _mockAuditoriumRepository;
    private readonly IAuditoriumService _auditoriumService;

    public AuditoriumServicesTests()
    {
        _mockAuditoriumRepository = new Mock<IAuditoriumsRepository>();
        _auditoriumService = new AuditoriumService(_mockAuditoriumRepository.Object);
    }

    [Fact(DisplayName = "Should throw exception not found when auditorium does not exist")]
    public async Task IsAuditoriumAvailable_WithAuditoriumNotFound_ShouldThrowException()
    {
        //Arrange
        _mockAuditoriumRepository.Setup(r =>
            r.GetAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>())).ReturnsAsync((AuditoriumEntity)null);

        //Act
        var action = async () => await _auditoriumService.IsAuditoriumAvailable(1, DateTime.Now);

        //Assert
        action.Should().ThrowAsync<AuditoriumNotFoundException>().WithMessage("Auditorium not found.");
    }

    [Fact(DisplayName = "Should return true when auditorium is available")]
    public async Task IsAuditoriumAvailable_WithAuditoriumAvailable_ShouldReturnTrue()
    {
        //Arrange
        var repoAuditorium = new AuditoriumEntity()
        {
            Id = 1,
            Seats = GenerateSeats(1, 20, 20),
            Showtimes = new List<ShowtimeEntity>()
            {
                new ShowtimeEntity()
                {
                    Id = 1,
                    SessionDate = new DateTime(2020, 1, 1)
                }
            }
        };

        _mockAuditoriumRepository.Setup(r =>
            r.GetAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>())).ReturnsAsync(repoAuditorium);

        //Act
        var result = await _auditoriumService.IsAuditoriumAvailable(1, DateTime.Now);

        //Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = "Should return false when auditorium is not available")]
    public async Task IsAuditoriumAvailable_WithAuditoriumNotAvailable_ShouldReturnFalse()
    {
        //Arrange
        var repoAuditorium = new AuditoriumEntity()
        {
            Id = 1,
            Seats = GenerateSeats(1, 20, 20),
            Showtimes = new List<ShowtimeEntity>()
            {
                new ShowtimeEntity()
                {
                    Id = 1,
                    SessionDate = new DateTime(2020, 1, 1)
                }
            }
        };

        _mockAuditoriumRepository.Setup(r =>
            r.GetAsync(
                It.IsAny<int>(),
                It.IsAny<CancellationToken>(),
                It.IsAny<bool>())).ReturnsAsync(repoAuditorium);

        //Act
        var result = await _auditoriumService.IsAuditoriumAvailable(1, new DateTime(2020, 1, 1));

        //Assert
        result.Should().BeFalse();
    }


    #region helpers

    private static List<SeatEntity> GenerateSeats(int auditoriumId, short rows, short seatsPerRow)
    {
        var seats = new List<SeatEntity>();
        for (short r = 1; r <= rows; r++)
        for (short s = 1; s <= seatsPerRow; s++)
            seats.Add(new SeatEntity { AuditoriumId = auditoriumId, Row = r, SeatNumber = s });

        return seats;
    }

    #endregion
}