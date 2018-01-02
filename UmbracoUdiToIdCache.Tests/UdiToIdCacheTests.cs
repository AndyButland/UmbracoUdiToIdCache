namespace UmbracoUdiToIdCache.Tests
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Umbraco.Core;
    using Umbraco.Core.Models;

    [TestClass]
    public class UdiToIdCacheTests
    {
        protected IContent MockContent(int id, Guid key)
        {
            var mock = new Mock<IContent>();
            mock.SetupGet(x => x.Id).Returns(id);
            mock.SetupGet(x => x.Key).Returns(key);
            return mock.Object;
        }

        [TestClass]
        public class TheAddToCacheMethod : UdiToIdCacheTests
        {
            [TestMethod]
            public void ShouldAddKeyValuePairIfDoesNotAlreadyExist()
            {
                // Arrange
                UdiToIdCache.Clear();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);
                UdiToIdCache.AddToCache(content);

                // Act
                var result = UdiToIdCache.TryGetId(content.GetUdi(), out int id);

                // Assert
                result.Should().BeTrue();
                id.Should().Be(1001);
            }

            [TestMethod]
            public void ShouldRetainKeyValuePairIfAlreadyExists()
            {
                // Arrange
                UdiToIdCache.Clear();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);
                UdiToIdCache.AddToCache(content);
                UdiToIdCache.AddToCache(content);

                // Act
                var result = UdiToIdCache.TryGetId(content.GetUdi(), out int id);

                // Assert
                result.Should().BeTrue();
                id.Should().Be(1001);
            }
        }

        [TestClass]
        public class TheTryGetIdMethod : UdiToIdCacheTests
        {
            [TestMethod]
            public void ShouldRetrieveIdIfMappingExists()
            {
                // Arrange
                UdiToIdCache.Clear();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);
                UdiToIdCache.AddToCache(content);

                // Act
                var result = UdiToIdCache.TryGetId(content.GetUdi(), out int id);

                // Assert
                result.Should().BeTrue();
                id.Should().Be(1001);
            }

            [TestMethod]
            public void ShouldNotRetrieveIdIfMappingDoesNotExist()
            {
                // Arrange
                UdiToIdCache.Clear();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);

                // Act
                var result = UdiToIdCache.TryGetId(content.GetUdi(), out int id);

                // Assert
                result.Should().BeFalse();
                id.Should().Be(0);
            }
        }
    }
}
