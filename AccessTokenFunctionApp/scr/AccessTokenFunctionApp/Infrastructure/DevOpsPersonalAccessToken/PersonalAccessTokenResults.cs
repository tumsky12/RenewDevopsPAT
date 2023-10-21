using System.Text.Json.Serialization;

namespace AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;

public class PersonalAccessTokenResults
{
    [JsonInclude]
    [JsonPropertyName("patToken")]
    public PersonalAccessToken? PersonalAccessToken { get; private set; }

    [JsonInclude]
    [JsonPropertyName("patTokenError")]
    public string PersonalAccessTokenError { get; private set; } = "none";
}
