using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Our.Umbraco.UdiCache;
using Umbraco.Core.Models;

namespace Our.Umbraco.UdiCache.Tests
{
    [TestClass]
    public class GuidToIdCacheTests
    {
        protected IContent MockContent(int id, Guid key)
        {
            var mock = new Mock<IContent>();
            mock.SetupGet(x => x.Id).Returns(id);
            mock.SetupGet(x => x.Key).Returns(key);
            return mock.Object;
        }

        [TestClass]
        public class TheTryAddMethod : GuidToIdCacheTests
        {
            [TestMethod]
            public void ShouldAddKeyValuePairIfDoesNotAlreadyExist()
            {
                // Arrange
                GuidToIdCache.ClearAll();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);
                GuidToIdCache.TryAdd(content);

                // Act
                var result = GuidToIdCache.TryGetId(content.Key, out int id);

                // Assert
                result.Should().BeTrue();
                id.Should().Be(1001);
            }

            [TestMethod]
            public void ShouldRetainKeyValuePairIfAlreadyExists()
            {
                // Arrange
                GuidToIdCache.ClearAll();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);
                GuidToIdCache.TryAdd(content);
                GuidToIdCache.TryAdd(content);

                // Act
                var result = GuidToIdCache.TryGetId(content.Key, out int id);

                // Assert
                result.Should().BeTrue();
                id.Should().Be(1001);
            }
        }

        [TestClass]
        public class TheTryGetIdMethod : GuidToIdCacheTests
        {
            [TestMethod]
            public void ShouldRetrieveIdIfMappingExists()
            {
                // Arrange
                GuidToIdCache.ClearAll();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);
                GuidToIdCache.TryAdd(content);

                // Act
                var result = GuidToIdCache.TryGetId(content.Key, out int id);

                // Assert
                result.Should().BeTrue();
                id.Should().Be(1001);
            }

            [TestMethod]
            public void ShouldNotRetrieveIdIfMappingDoesNotExist()
            {
                // Arrange
                GuidToIdCache.ClearAll();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);

                // Act
                var result = GuidToIdCache.TryGetId(content.Key, out int id);

                // Assert
                result.Should().BeFalse();
                id.Should().Be(0);
            }
        }

        [TestClass]
        public class TheTryGetGuidMethod : GuidToIdCacheTests
        {
            [TestMethod]
            public void ShouldRetrieveGuidIfMappingExists()
            {
                // Arrange
                GuidToIdCache.ClearAll();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);
                GuidToIdCache.TryAdd(content);

                // Act
                var result = GuidToIdCache.TryGetGuid(content.Id, out Guid guid);

                // Assert
                result.Should().BeTrue();
                guid.Should().Be(key);
            }

            [TestMethod]
            public void ShouldNotRetrieveGuidIfMappingDoesNotExist()
            {
                // Arrange
                GuidToIdCache.ClearAll();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);

                // Act
                var result = GuidToIdCache.TryGetGuid(content.Id, out Guid guid);

                // Assert
                result.Should().BeFalse();
                guid.Should().Be(Guid.Empty);
            }
        }

        [TestClass]
        public class TheTryRemoveMethod : GuidToIdCacheTests
        {
            [TestMethod]
            public void ShouldRemoveKeyValuePairAndNotRetrieveId()
            {
                // Arrange
                GuidToIdCache.ClearAll();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);
                GuidToIdCache.TryAdd(content);

                // Act
                GuidToIdCache.TryRemove(content);
                var result = GuidToIdCache.TryGetId(content.Key, out int id);

                // Assert
                result.Should().BeFalse();
                id.Should().Be(0);
            }

            [TestMethod]
            public void ShouldRemoveKeyValuePairAndNotRetrieveGuid()
            {
                // Arrange
                GuidToIdCache.ClearAll();
                var key = Guid.NewGuid();
                var content = MockContent(1001, key);
                GuidToIdCache.TryAdd(content);

                // Act
                GuidToIdCache.TryRemove(content);
                var result = GuidToIdCache.TryGetGuid(content.Id, out Guid guid);

                // Assert
                result.Should().BeFalse();
                guid.Should().Be(Guid.Empty);
            }
        }
    }
}