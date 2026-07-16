using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoviCode.Tests.Services
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _playerRepositoryMock = new();
        private readonly Mock<ICache> _cacheMock = new();
        private readonly Mock<ILogger<PlayerService>> _loggerMock = new();
        private readonly PlayerService _sut;
        public PlayerServiceTests()
        {
            _sut = new(_playerRepositoryMock.Object, _cacheMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetById_IdExists_ReturnsPlayer()
        {
            //Arrange
            var expectedPlayer = Player.CreateNew("name");
            //_cacheMock.Setup(mock => mock.TryGet(It.IsAny<string>(),));
            _playerRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expectedPlayer);
            //Act
            var player = await _sut.GetByIdAsync(Guid.NewGuid());
            //Assert
            Assert.NotNull(player);
            Assert.Equal(expectedPlayer, player);
        }
    }
}
