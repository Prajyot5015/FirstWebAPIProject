using AutoMapper;
using FirstWebAPIProject.Controllers;
using FirstWebAPIProject.Model.Domain;
using FirstWebAPIProject.Model.DTO;
using FirstWebAPIProject.Repository.Interface;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace FirstWebAPIProject.Tests.Controllers
{
    public class WalksControllerTests
    {
         private readonly Mock<IWalksRepository> _walksRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly WalksController _controller;

        public WalksControllerTests()
        {
            _walksRepositoryMock = new Mock<IWalksRepository>();
            _mapperMock = new Mock<IMapper>();

            _controller = new WalksController(_mapperMock.Object, _walksRepositoryMock.Object);
        }

        // Test for GetAllWalks
        [Fact]
        public async Task GetAllWalks_ShouldReturnOkResult_WithListOfWalks()
        {
            // Arrange
            var walks = new List<Walk> { new Walk { Id = Guid.NewGuid(), Name = "Mountain Trail" } };
            var walkDtos = new List<WalkDTO> { new WalkDTO { Id = walks[0].Id, Name = "Mountain Trail" } };

            _walksRepositoryMock.Setup(repo => repo.GetAllWalksAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(walks);
            _mapperMock.Setup(m => m.Map<List<WalkDTO>>(walks)).Returns(walkDtos);

            // Act
            var result = await _controller.GetAllWalks(null, null, null, true, 1, 10);

            // Assert

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<List<WalkDTO>>();
            ((List<WalkDTO>)okResult.Value).Should().HaveCount(1);
        }
    }
}
