using Moq;
using AccessTokenFunctionApp.Infrastructure;
using AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;
using AccessTokenFunctionApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace AccessTokenFunctionApp.UnitTests.AccessTokenFunctionApp.Infrastructure
{
    [TestFixture]
    public class DevOpsPatRenewerTests
    {
        private readonly Mock<IDevOpsPatApiWrapper> _devOpsPatApiWrapperMock = new();
        private readonly Mock<ILogger<DevOpsPatRenewer>> _loggerMock = new();
        private IDevOpsPatRenewer _devOpsPatRenewer = null!;

        [SetUp]
        public void Setup()
        {
            _devOpsPatRenewer = new DevOpsPatRenewer(_devOpsPatApiWrapperMock.Object, _loggerMock.Object);
        }

        [TestCase("Token-1230571-5-random-StRiNg")]
        [TestCase(null)]
        public async Task RenewPat_TokenReturned_Matches_API_Response(string? expectedToken)
        {
            // Arrange
            var tokenResponse = GetNewPersonalAccessTokenResultsWithValue(expectedToken);

            _devOpsPatApiWrapperMock.Setup(x =>
                    x.CreateAsync(It.IsAny<CreatePersonalAccessTokenOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tokenResponse);

            // Act
            var actualToken = await _devOpsPatRenewer.Renew();

            // Assert
            Assert.That(actualToken, Is.EqualTo(expectedToken));
        }

        private static PersonalAccessTokenResults GetNewPersonalAccessTokenResultsWithValue(string? tokenValue)
        {
            var newToken = NewInstanceWithSetProperty<PersonalAccessToken, string>(nameof(PersonalAccessToken.Token), tokenValue);
            var newPersonalAccessTokenResults = NewInstanceWithSetProperty<PersonalAccessTokenResults, PersonalAccessToken>(nameof(PersonalAccessTokenResults.PersonalAccessToken), newToken);
            return newPersonalAccessTokenResults;
        }

        private static TReturn NewInstanceWithSetProperty<TReturn, TValue>(string propertyName, TValue? value)
            where TReturn : new()
        {
            var newReturn = new TReturn();
            newReturn.GetType().GetProperty(propertyName)?.SetValue(newReturn, value);
            return newReturn;
        }

    }
}
