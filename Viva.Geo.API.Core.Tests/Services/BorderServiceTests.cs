using System.Linq.Expressions;
using AutoMapper;
using Common.Logging.Serilog.Factories;
using Common.MemoryCaching.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using Viva.Geo.API.Common.Dtos.Borders.Responses;
using Viva.Geo.API.Core.Abstractions.Repositories;
using Viva.Geo.API.Core.Services;
using Viva.Geo.API.DataAccess.DataAccessModels;

namespace Viva.Geo.API.Core.Tests.Services;

public class BorderServiceTests
{
    private readonly Mock<IBorderRepository> _mockBorderRepository;
    private readonly Mock<IMemoryCacheService> _mockCacheService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly BorderService _borderService;

    public BorderServiceTests()
    {
        var mockLogger = new Mock<ILogger<BorderService>>();

        _mockBorderRepository = new Mock<IBorderRepository>();
        _mockCacheService = new Mock<IMemoryCacheService>();
        _mockMapper = new Mock<IMapper>();
        _borderService = new BorderService(
            _mockBorderRepository.Object,
            _mockCacheService.Object,
            _mockMapper.Object,
            mockLogger.Object,
            new EventIdFactory()
        );
    }

    /// <summary>
    /// Test to ensure that when a border is found in cache, it is returned without querying the repository.
    /// </summary>
    [Fact]
    public async Task CreateOrUpdateBorderAsync_ShouldReturnCachedBorder_WhenCacheHit()
    {
        // Arrange
        const string borderCode = "ABC";
        var cachedBorderDto = new BorderDto {BorderCode = borderCode};
        _mockCacheService.Setup(s => s.Get<BorderDto>(It.IsAny<string>())).Returns(cachedBorderDto);

        // Act
        var result =
            await _borderService.CreateOrUpdateBorderAsync(new Border {BorderCode = borderCode},
                CancellationToken.None);

        // Assert
        Assert.Equal(cachedBorderDto, result);
        _mockBorderRepository.Verify(
            x => x.SearchAsync(It.IsAny<Expression<Func<Border, bool>>>(), It.IsAny<CancellationToken>()), Times.Never);
    }


    /// <summary>
    /// Tests that the CreateOrUpdateBorderAsync method returns an existing border from the database when it's not found in the cache.
    /// </summary>
    [Fact]
    public async Task CreateOrUpdateBorderAsync_ShouldReturnBorderFromDatabase_WhenCacheMissAndDatabaseHit()
    {
        // Arrange
        const string borderCode = "XYZ";
        var databaseBorder = new Border {BorderCode = borderCode};
        var borderDto = new BorderDto {BorderCode = borderCode};
        _mockCacheService.Setup(s => s.Get<BorderDto>(It.IsAny<string>())).Returns((BorderDto) null);
        _mockBorderRepository.Setup(r =>
                r.SearchAsync(It.IsAny<Expression<Func<Border, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] {databaseBorder});
        _mockMapper.Setup(m => m.Map<BorderDto>(It.IsAny<Border>())).Returns(borderDto);

        // Act
        var result =
            await _borderService.CreateOrUpdateBorderAsync(new Border {BorderCode = borderCode},
                CancellationToken.None);

        // Assert
        Assert.Equal(borderDto, result);
        _mockCacheService.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<BorderDto>(), It.IsAny<TimeSpan>()),
            Times.Once);
    }


    /// <summary>
    /// Tests that CreateOrUpdateBorderAsync creates a new border and returns its DTO
    /// when both cache and database do not have the border.
    /// </summary>
    [Fact]
    public async Task CreateOrUpdateBorderAsync_WhenCacheAndDatabaseMiss_ShouldCreateAndReturnNewBorder()
    {
        // Arrange
        const string borderCode = "DEF";
        var newBorder = new Border {BorderCode = borderCode};
        var borderDto = new BorderDto {BorderCode = borderCode};
        _mockCacheService.Setup(s => s.Get<BorderDto>(It.IsAny<string>())).Returns((BorderDto) null);
        _mockBorderRepository.Setup(r =>
                r.SearchAsync(It.IsAny<Expression<Func<Border, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<Border>());
        _mockMapper.Setup(m => m.Map<BorderDto>(It.IsAny<Border>())).Returns(borderDto);
        _mockBorderRepository.Setup(r => r.CreateAsync(It.IsAny<Border>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newBorder);

        // Act
        var result =
            await _borderService.CreateOrUpdateBorderAsync(new Border {BorderCode = borderCode},
                CancellationToken.None);

        // Assert
        Assert.Equal(borderDto, result);
        _mockCacheService.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<BorderDto>(), It.IsAny<TimeSpan>()),
            Times.Once);
        _mockBorderRepository.Verify(r => r.CreateAsync(It.IsAny<Border>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>
    /// Tests that the service correctly queries the database and returns an existing border if found.
    /// </summary>
    [Fact]
    public async Task CreateOrUpdateBorderAsync_ShouldQueryDatabaseAndReturnBorder_WhenCacheMissAndDatabaseHit()
    {
        // Arrange
        const string borderCode = "XYZ";
        var databaseBorder = new Border {BorderCode = borderCode};
        var borderDto = new BorderDto {BorderCode = borderCode};
        _mockCacheService.Setup(s => s.Get<BorderDto>(It.IsAny<string>())).Returns((BorderDto) null);
        _mockBorderRepository.Setup(r =>
                r.SearchAsync(It.IsAny<Expression<Func<Border, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new[] {databaseBorder});
        _mockMapper.Setup(m => m.Map<BorderDto>(It.IsAny<Border>())).Returns(borderDto);

        // Act
        var result =
            await _borderService.CreateOrUpdateBorderAsync(new Border {BorderCode = borderCode},
                CancellationToken.None);

        // Assert
        Assert.Equal(borderDto, result);
        _mockCacheService.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<BorderDto>(), It.IsAny<TimeSpan>()),
            Times.Once);
    }

    /// <summary>
    /// Tests that the service correctly creates and returns a new border if it does not exist in the cache or database.
    /// </summary>
    [Fact]
    public async Task CreateOrUpdateBorderAsync_ShouldCreateAndReturnNewBorder_WhenCacheAndDatabaseMiss()
    {
        // Arrange
        const string borderCode = "DEF";
        var newBorder = new Border {BorderCode = borderCode};
        var borderDto = new BorderDto {BorderCode = borderCode};
        _mockCacheService.Setup(s => s.Get<BorderDto>(It.IsAny<string>())).Returns((BorderDto) null);
        _mockBorderRepository.Setup(r =>
                r.SearchAsync(It.IsAny<Expression<Func<Border, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<Border>());
        _mockMapper.Setup(m => m.Map<BorderDto>(It.IsAny<Border>())).Returns(borderDto);
        _mockBorderRepository.Setup(r => r.CreateAsync(It.IsAny<Border>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newBorder);

        // Act
        var result =
            await _borderService.CreateOrUpdateBorderAsync(new Border {BorderCode = borderCode},
                CancellationToken.None);

        // Assert
        Assert.Equal(borderDto, result);
        _mockCacheService.Verify(s => s.Set(It.IsAny<string>(), It.IsAny<BorderDto>(), It.IsAny<TimeSpan>()),
            Times.Once);
        _mockBorderRepository.Verify(r => r.CreateAsync(It.IsAny<Border>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}