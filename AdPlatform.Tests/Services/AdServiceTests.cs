using AdPlatform.Application.Interfaces;
using AdPlatform.Application.Services;
using AdPlatform.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using NUnit.Framework;
using System.Text;

namespace AdPlatform.Tests.Services
{
    public class AdServiceTests
    {
        private AdService _service;
        private Mock<IAdPlatformRepository> _repoMock;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IAdPlatformRepository>();
            _service = new AdService(_repoMock.Object);
        }

        [Test]
        public void LoadAdPlatformsFromFile_AddsPlatformToRepository() 
        {
            // Arrange
            var content = "Test.Platform:/ru/msk,/ru/spb";
            var fileBytes = Encoding.UTF8.GetBytes(content);
            var file = new FormFile(
                baseStream: new MemoryStream(fileBytes),
                baseStreamOffset: 0,
                length: fileBytes.Length,
                name: "file",
                fileName: "platforms.txt"
            );

            var expectedPlatforms = new List<Platform>
            {
                new Platform { Name = "Test.Platform" }
            };

            _repoMock.Setup(repo => repo.AddLoadedDataToDb(It.IsAny<Dictionary<string, string[]>>()))
                     .Returns(expectedPlatforms);

            // Act
            var result = _service.LoadAdPlatformsFromFile(file).ToList();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(1, Is.EqualTo(result.Count()));
            Assert.That("Test.Platform", Is.EqualTo(result.ElementAt(0).Name));

            _repoMock.Verify(repo => repo.AddLoadedDataToDb(It.IsAny<Dictionary<string, string[]>>()), Times.Once);

        }

        [Test]
        public void GetAdPlatformsByLocation_ReturnsPlatforms_WhenLocationMatches()
        {
            // Arrange
            var location = "/test/location";
            var platforms = new List<Platform>
            {
                new Platform
                {
                    Name = "Test.Platform"
                }
            };

            _repoMock.Setup(r => r.GetAdPlatformsByLocation(location)).Returns(platforms);

            // Act
            var result = _service.GetAddPlatformsByLocation(location);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(1, Is.EqualTo(result.Count()));
            Assert.That("Test.Platform", Is.EqualTo(result.ElementAt(0).Name));
        }
    }
}
