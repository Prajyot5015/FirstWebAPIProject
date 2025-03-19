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
        private readonly Mock<IWalksRepository> _walksRepositoryMock; // Mocking the IWalksRepository interface  
        private readonly Mock<IMapper> _mapperMock; // Mocking the IMapper interface
        private readonly WalksController _controller; // Creating an instance of WalksController

        public WalksControllerTests()
        {
            _walksRepositoryMock = new Mock<IWalksRepository>(); // Mocking the IWalksRepository interface 
            _mapperMock = new Mock<IMapper>(); // Mocking the IMapper interface 

            _controller = new WalksController(_mapperMock.Object, _walksRepositoryMock.Object); // Creating an instance of WalksController with the mocked IMapper and IWalksRepository 
        }

        // Test for GetAllWalks
        [Fact]
        public async Task GetAllWalks_ShouldReturnOkResult_WithListOfWalks() // It checks if GetAllWalks returns OkObjectResult with a list of WalkDTO.
        {
            // Arrange
            var walks = new List<Walk> { new Walk { Id = Guid.NewGuid(), Name = "Mountain Trail" } }; // Creating a list of Walks with one Walk object 
            var walkDtos = new List<WalkDTO> { new WalkDTO { Id = walks[0].Id, Name = "Mountain Trail" } }; // Creating a list of WalkDTOs with one WalkDTO object

            _walksRepositoryMock.Setup(repo => repo.GetAllWalksAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(walks); // Setting up the GetAllWalksAsync method of the IWalksRepository to return the list of Walks 
           
            _mapperMock.Setup(m => m.Map<List<WalkDTO>>(walks)).Returns(walkDtos);

            // Act
            var result = await _controller.GetAllWalks(null, null, null, true, 1, 10); // Calling the GetAllWalks method of WalksController with the parameters

            // Assert

            result.Should().BeOfType<OkObjectResult>(); // Checking if the result is of type OkObjectResult 
            var okResult = result as OkObjectResult; // Casting the result to OkObjectResult
            okResult.Value.Should().BeOfType<List<WalkDTO>>(); // Checking if the value of the result is of type List<WalkDTO> 
            ((List<WalkDTO>)okResult.Value).Should().HaveCount(1); // Checking if the value of the result has a count of 1 
        }


        // Test for GetWalkById (Walk Exists)
        [Fact]
        public async Task GetWalkById_ShouldReturnOk_WhenWalkExists()
        {
            // Arrange
            var walk = new Walk { Id = Guid.NewGuid(), Name = "Forest Walk" }; // Creating a Walk object with a name "Forest Walk" 
            var walkDto = new WalkDTO { Id = walk.Id, Name = "Forest Walk" }; // Creating a WalkDTO object with a name "Forest Walk"    

            _walksRepositoryMock.Setup(repo => repo.GetWalkByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(walk); // Setting up the GetWalkByIdAsync method of the IWalksRepository to return the Walk object 
            _mapperMock.Setup(m => m.Map<WalkDTO>(walk)).Returns(walkDto); // Setting up the Map method of the IMapper to return the WalkDTO object 

            // Act
            var result = await _controller.GetWalkById(walk.Id); // Calling the GetWalkById method of WalksController with the parameter

            // Assert
            result.Should().BeOfType<OkObjectResult>(); // Checking if the result is of type OkObjectResult 
            var okResult = result as OkObjectResult; // Casting the result to OkObjectResult 
            okResult.Value.Should().BeOfType<WalkDTO>();  // Checking if the value of the result is of type WalkDTO
            ((WalkDTO)okResult.Value).Name.Should().Be("Forest Walk"); // Checking if the name of the WalkDTO object is "Forest Walk" 
        }


        // Test for GetWalkById (Walk Not Found)
        [Fact]
        public async Task GetWalkById_ShouldReturnNotFound_WhenWalkDoesNotExist()
        {
            // Arrange
            _walksRepositoryMock.Setup(repo => repo.GetWalkByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Walk)null);

            // Act
            var result = await _controller.GetWalkById(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }


        // Test for AddWalk
        [Fact]
        public async Task AddWalk_ShouldReturnOk_WithCreatedWalk()
        {
            // Arrange
            var addWalkDto = new AddWalkDTO { Name = "River Walk" };
            var walkDomainModel = new Walk { Id = Guid.NewGuid(), Name = "River Walk" };
            var walkDto = new WalkDTO { Id = walkDomainModel.Id, Name = "River Walk" };

            _mapperMock.Setup(m => m.Map<Walk>(addWalkDto)).Returns(walkDomainModel);
            _walksRepositoryMock.Setup(repo => repo.AddWalkAsync(walkDomainModel))
                .ReturnsAsync(walkDomainModel);
            _mapperMock.Setup(m => m.Map<WalkDTO>(walkDomainModel)).Returns(walkDto);

            // Act
            var result = await _controller.AddWalk(addWalkDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<WalkDTO>();
            ((WalkDTO)okResult.Value).Name.Should().Be("River Walk");
        }

        // Test for UpdateWalk
        [Fact]
        public async Task UpdateWalk_ShouldReturnOk_WhenWalkExists()
        {
            // Arrange
            var updateWalkDto = new UpdateWalkDTO { Name = "Updated Walk" };
            var walkDomainModel = new Walk { Id = Guid.NewGuid(), Name = "Updated Walk" };
            var walkDto = new WalkDTO { Id = walkDomainModel.Id, Name = "Updated Walk" };

            _mapperMock.Setup(m => m.Map<Walk>(updateWalkDto)).Returns(walkDomainModel);
            _walksRepositoryMock.Setup(repo => repo.UpdateWalkAsync(It.IsAny<Guid>(), It.IsAny<Walk>()))
                .ReturnsAsync(walkDomainModel);
            _mapperMock.Setup(m => m.Map<WalkDTO>(walkDomainModel)).Returns(walkDto);

            // Act
            var result = await _controller.UpdateWalk(Guid.NewGuid(), updateWalkDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<WalkDTO>();
            ((WalkDTO)okResult.Value).Name.Should().Be("Updated Walk");
        }

        // Test for DeleteWalk (Walk Exists)
        [Fact]
        public async Task DeleteWalk_ShouldReturnOk_WhenWalkExists()
        {
            // Arrange
            var walk = new Walk { Id = Guid.NewGuid(), Name = "Lake Walk" };
            var walkDto = new WalkDTO { Id = walk.Id, Name = "Lake Walk" };

            _walksRepositoryMock.Setup(repo => repo.DeleteWalkAsync(It.IsAny<Guid>()))
                .ReturnsAsync(walk);
            _mapperMock.Setup(m => m.Map<WalkDTO>(walk)).Returns(walkDto);

            // Act
            var result = await _controller.DeleteWalk(walk.Id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<WalkDTO>();
            ((WalkDTO)okResult.Value).Name.Should().Be("Lake Walk");
        }

        // Test for DeleteWalk (Walk Not Found)
        [Fact]
        public async Task DeleteWalk_ShouldReturnNotFound_WhenWalkDoesNotExist()
        {
            // Arrange
            _walksRepositoryMock.Setup(repo => repo.DeleteWalkAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Walk)null);

            // Act
            var result = await _controller.DeleteWalk(Guid.NewGuid());

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

    }
}
