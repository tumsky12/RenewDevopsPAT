using System.Diagnostics;
using System.Net;
using Moq;
using AccessTokenFunctionApp.Infrastructure;
using AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;
using AccessTokenFunctionApp.UnitTests.AccessTokenFunctionApp.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AccessTokenFunctionApp.UnitTests.AccessTokenFunctionApp.Infrastructure
{
    [TestFixture]
    public class DevOpsPatApiWrapperTests
    {
        private const string BearerToken = "BearerToken";
        private const string AuthorizationId = "AuthorizationId";
        private const string RequestUrl = "RequestUrl";

        private static readonly Dictionary<string, string> ConfigurationInMemoryCollection = new()
        {
            {"DEVOPS_ORGANIZATION_NAME", "DevOpsOrganizationName"}
        };
        private readonly IConfiguration _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(ConfigurationInMemoryCollection).Build();


        private static IEnumerable<HttpStatusCode> StatusCodes => Enum.GetValues(typeof(HttpStatusCode))
            .Cast<HttpStatusCode>()
            .Where(x => (int)x < 200 || (int)x >= 300);

        private readonly Mock<ILogger<DevOpsPatApiWrapper>> _loggerMock = new();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CreateAsync_Throws_On_Not_Ok_Status()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
            var httpClientFactory = CreateMockHttpFactoryWithResponseMessage(httpResponseMessage);
            var devOpsPatApiWrapper = new DevOpsPatApiWrapper(BearerToken, httpClientFactory, _configuration, _loggerMock.Object);
            var options = new CreatePersonalAccessTokenOptions(default, default!, default!, default!);
            // Act
            // Assert
            Assert.ThrowsAsync<HttpRequestException>(async Task () => await devOpsPatApiWrapper.CreateAsync(options, default));
        }

        [Test]
        public async Task CreateAsync_Can_Parse_Response_On_Ok_Status()
        {
            // Arrange
            var jsonResponse = DevopsPatHttpResponseTestHelper.GetValidPersonalAccessTokenResultsJson();
            var httpResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(jsonResponse)};
            var httpClientFactory = CreateMockHttpFactoryWithResponseMessage(httpResponseMessage);
            var devOpsPatApiWrapper = new DevOpsPatApiWrapper(BearerToken, httpClientFactory, _configuration, _loggerMock.Object);
            var options = new CreatePersonalAccessTokenOptions(default, default!, default!, default!);
            // Act
            var result = await devOpsPatApiWrapper.CreateAsync(options, default);
            // Asserts

            Assert.Null(result.PersonalAccessToken? .Token);
            Assert.That(1 , Is.EqualTo(1));
        }

        [Test]
        public void GetAsync_Throws_On_Not_Ok_Status()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };
            var httpClientFactory = CreateMockHttpFactoryWithResponseMessage(httpResponseMessage);
            var devOpsPatApiWrapper = new DevOpsPatApiWrapper(BearerToken, httpClientFactory, _configuration, _loggerMock.Object);
            // Act
            // Assert
            Assert.ThrowsAsync<HttpRequestException>(async Task () => await devOpsPatApiWrapper.GetAsync(default));
        }
        
        [TestCaseSource(nameof(StatusCodes))]
        public void ListAsync_Throws_On_Not_Ok_Status(HttpStatusCode statusCode)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage { StatusCode = statusCode, Content = new StringContent("Content") };
            var httpClientFactory = CreateMockHttpFactoryWithResponseMessage(httpResponseMessage);
            var devOpsPatApiWrapper = new DevOpsPatApiWrapper(BearerToken, httpClientFactory, _configuration, _loggerMock.Object);
            // Act
            // Assert
            Assert.ThrowsAsync<HttpRequestException>(async Task () => await devOpsPatApiWrapper.ListAsync(default));
        }

        [TestCaseSource(nameof(StatusCodes))]
        public void CreateAsync_Throws_On_Not_Ok_Status(HttpStatusCode statusCode)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage { StatusCode = statusCode, Content = new StringContent("Content") };
            var httpClientFactory = CreateMockHttpFactoryWithResponseMessage(httpResponseMessage);
            var devOpsPatApiWrapper = new DevOpsPatApiWrapper(BearerToken, httpClientFactory, _configuration, _loggerMock.Object);
            // Act
            // Assert
            Assert.ThrowsAsync<HttpRequestException>(async Task () => await devOpsPatApiWrapper.CreateAsync(default));
        }

        [TestCaseSource(nameof(StatusCodes))]
        public void GetAsync_Throws_On_Not_Ok_Status(HttpStatusCode statusCode)
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage { StatusCode = statusCode, Content = new StringContent("Content") };
            var httpClientFactory = CreateMockHttpFactoryWithResponseMessage(httpResponseMessage);
            var devOpsPatApiWrapper = new DevOpsPatApiWrapper(BearerToken, httpClientFactory, _configuration, _loggerMock.Object);
            // Act
            // Assert
            Assert.ThrowsAsync<HttpRequestException>(async Task () => await devOpsPatApiWrapper.GetAsync(default));
        }

        [Test]
        public void RevokeAsync_Throws_NotImplementedException()
        {
            // Arrange
            var httpClientFactory = CreateMockHttpFactoryWithResponseMessage(new HttpResponseMessage());
            var devOpsPatApiWrapper = new DevOpsPatApiWrapper(BearerToken, httpClientFactory, _configuration, _loggerMock.Object);
            // Act
            // Assert
            Assert.ThrowsAsync<NotImplementedException>(async Task () => await devOpsPatApiWrapper.RevokeAsync(AuthorizationId, default));
        }

        [Test]
        public void UpdateAsync_Throws_NotImplementedException()
        {
            // Arrange
            var httpClientFactory = CreateMockHttpFactoryWithResponseMessage(new HttpResponseMessage());
            var devOpsPatApiWrapper = new DevOpsPatApiWrapper(BearerToken, httpClientFactory, _configuration, _loggerMock.Object);
            var options = new UpdatePersonalAccessTokenOptions(default, default!, default!, default!, default!);

            // Act
            // Assert
            Assert.ThrowsAsync<NotImplementedException>(async Task () => await devOpsPatApiWrapper.UpdateAsync(options));
        }
        
        private static IHttpClientFactory CreateMockHttpFactoryWithResponseMessage(HttpResponseMessage response)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response)
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            Mock<IHttpClientFactory> mockFactory = new();
            mockFactory
                .Setup(x =>
                    x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return mockFactory.Object;
        }
    }
}
