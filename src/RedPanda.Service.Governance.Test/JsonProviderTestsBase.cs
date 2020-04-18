using FluentAssertions;
using RedPanda.Service.Governance.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Xunit;

namespace RedPanda.Service.Governance.Test
{
    enum TestEnum
    {
        RedPanda,
    }

    class ADto
    {
        public string RedPanda { get; set; }

        public bool Happy { get; set; }

        public int Year { get; set; }

        public TestEnum Enum { get; set; }

        public BDto BDto { get; set; }

        public CDto[] CDtos { get; set; }
    }

    public class BDto
    {
        public string Service { get; set; }
    }

    public class CDto
    {
        public string Governance { get; set; }
    }

    public abstract class JsonProviderTestsBase
    {
        protected readonly IJsonProvider jsonProvider;

        public JsonProviderTestsBase(IJsonProvider jsonProvider)
        {
            this.jsonProvider = jsonProvider;
        }

        [Fact]
        public virtual void GetValueFromJson_Should_ReturnCorrectStringValue()
        {
            // Arrange
            var expectedValue = "Hello World!";
            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(new
            {
                a = new
                {
                    b = new
                    {
                        c = expectedValue
                    }
                }
            });
            var path = "a:b:c";

            // Assert
            var actualValue = jsonProvider.GetValueFromJson<string>(utf8Bytes, path);

            // Act
            actualValue.Should().Be(expectedValue);
        }

        [Fact]
        public virtual void GetValueFromJson_Should_ReturnCorrectIntValue()
        {
            // Arrange
            var expectedValue = 2020;
            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(new
            {
                a = new
                {
                    b = new
                    {
                        c = expectedValue
                    }
                }
            });
            var path = "a:b:c";

            // Assert
            var actualValue = jsonProvider.GetValueFromJson<int>(utf8Bytes, path);

            // Act
            actualValue.Should().Be(expectedValue);
        }

        [Fact]
        public virtual void GetValueFromJson_Should_ReturnCorrectBoaleanValue()
        {
            // Arrange
            var expectedValue = true;
            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(new
            {
                a = new
                {
                    b = new
                    {
                        c = expectedValue
                    }
                }
            });
            var path = "a:b:c";

            // Assert
            var actualValue = jsonProvider.GetValueFromJson<bool>(utf8Bytes, path);

            // Act
            actualValue.Should().Be(expectedValue);
        }

        [Fact]
        public virtual void GetValueFromJson_Should_ReturnCorrectEnumValueFromEnumString()
        {
            // Arrange

            var expectedValue = TestEnum.RedPanda;
            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(new
            {
                a = new
                {
                    b = new
                    {
                        c = expectedValue.ToString()
                    }
                }
            });
            var path = "a:b:c";

            // Assert
            var actualValue = jsonProvider.GetValueFromJson<TestEnum>(utf8Bytes, path);

            // Act
            actualValue.Should().Be(expectedValue);
        }

        [Fact]
        public virtual void GetValueFromJson_Should_ReturnCorrectEnumValueFromEnumNumber()
        {
            // Arrange

            var expectedValue = TestEnum.RedPanda;
            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(new
            {
                a = new
                {
                    b = new
                    {
                        c = expectedValue
                    }
                }
            });
            var path = "a:b:c";

            // Assert
            var actualValue = jsonProvider.GetValueFromJson<TestEnum>(utf8Bytes, path);

            // Act
            actualValue.Should().Be(expectedValue);
        }

        [Fact]
        public virtual void GetValueFromJson_Should_ThrowException_WhenThePathIsInvalid()
        {
            // Arrange
            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(new
            {
                a = new
                {
                    b = new
                    {
                        c = "Hello World!",
                    }
                }
            });
            var path = "a:d:c";

            // Assert
            Action act = () => jsonProvider.GetValueFromJson<TestEnum>(utf8Bytes, path);

            // Act
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public virtual void GetValueFromJson_Should_ThrowInvalidCastException_WhenValueConvertionFailed()
        {
            // Arrange
            var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(new
            {
                a = new
                {
                    b = new
                    {
                        c = new { d = "Hello World!" },
                    }
                }
            });
            var path = "a:b:c";

            // Assert
            Action act = () => jsonProvider.GetValueFromJson<string>(utf8Bytes, path);

            // Act
            act.Should().Throw<InvalidCastException>();
        }

        [Fact]
        public virtual void SerializeToUtf8Bytes_Should_ReturnUtf8BytesOfGenericInstancePassedIn()
        {
            // Arrange
            var testDto = new ADto
            {
                RedPanda = "Hello World!",
                Happy = true,
                Year = 2020,
                Enum = TestEnum.RedPanda,
                BDto = new BDto
                {
                    Service = "RedPanda"
                }
            };
            var expectedResult = Encoding.UTF8.GetBytes("{\"RedPanda\":\"Hello World!\",\"Happy\":true,\"Year\":2020,\"Enum\":0,\"BDto\":{\"Service\":\"RedPanda\"}}");

            // Assert
            var actualResult = jsonProvider.SerializeToUtf8Bytes(testDto);

            // Act
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public virtual void Deserialize_Should_ReturnGenericInstanceByUtf8BytesPassedIn()
        {
            // Arrange
            var utf8JsonBytes = Encoding.UTF8.GetBytes("{\"RedPanda\":\"Hello World!\",\"Happy\":true,\"Year\":2020,\"Enum\":0,\"BDto\":{\"Service\":\"RedPanda\"}}");
            var expectedResult = new ADto
            {
                RedPanda = "Hello World!",
                Happy = true,
                Year = 2020,
                Enum = TestEnum.RedPanda,
                BDto = new BDto
                {
                    Service = "RedPanda"
                }
            };

            // Assert
            var actualResult = jsonProvider.Deserialize<ADto>(utf8JsonBytes);

            // Act
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public virtual void DeserializeUtf8BytesToJson_Should_ReturnJsonStringByGenericTypeAndUtf8BytesPassedIn()
        {
            // Arrange
            var utf8JsonBytes = Encoding.UTF8.GetBytes("{\"RedPanda\":\"Hello World!\",\"Happy\":true,\"Year\":2020,\"Enum\":0,\"BDto\":{\"Service\":\"RedPanda\"}}");
            var expectedResult = JsonSerializer.Serialize(new ADto
            {
                RedPanda = "Hello World!",
                Happy = true,
                Year = 2020,
                Enum = TestEnum.RedPanda,
                BDto = new BDto
                {
                    Service = "RedPanda"
                }
            }, new JsonSerializerOptions { IgnoreNullValues = true });

            // Assert
            var actualResult = jsonProvider.DeserializeUtf8BytesToJson<ADto>(utf8JsonBytes);

            // Act
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public virtual void QueryByJsonPath_Should_ReturnValueAccordingToGenericValueTypeAndJsonPathPassedIn()
        {
            // Arrange
            var utf8JsonBytes = JsonSerializer.SerializeToUtf8Bytes(new ADto
            {
                RedPanda = "Hello World!",
                Happy = true,
                Year = 2020,
                Enum = TestEnum.RedPanda,
                BDto = new BDto
                {
                    Service = "RedPanda"
                },
                CDtos = new CDto[]
                {
                    new CDto
                    {
                        Governance = "Hot"
                    },
                    new CDto
                    {
                        Governance ="Cool"
                    },
                }
            });

            // Assert
            var stringResult = jsonProvider.QueryByJsonPath<string>(utf8JsonBytes, "$.RedPanda");
            var booleanResult = jsonProvider.QueryByJsonPath<bool>(utf8JsonBytes, "$.Happy");
            var intResult = jsonProvider.QueryByJsonPath<int>(utf8JsonBytes, "$.Year");
            var enumResult = jsonProvider.QueryByJsonPath<TestEnum>(utf8JsonBytes, "$.Enum");
            var innerStringResult = jsonProvider.QueryByJsonPath<string>(utf8JsonBytes, "$.BDto.Service");
            var innerArrayStringResult = jsonProvider.QueryByJsonPath<string>(utf8JsonBytes, "$.CDtos.[1].Governance");

            // Act
            stringResult.Should().BeEquivalentTo("Hello World!");
            booleanResult.Should().Be(true);
            intResult.Should().Be(2020);
            enumResult.Should().Be(TestEnum.RedPanda);
            innerStringResult.Should().Be("RedPanda");
            innerArrayStringResult.Should().Be("Cool");
        }
    }
}
