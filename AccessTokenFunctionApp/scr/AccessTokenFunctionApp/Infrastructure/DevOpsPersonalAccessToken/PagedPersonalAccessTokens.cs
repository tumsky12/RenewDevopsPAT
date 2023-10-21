using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;

public class PagedPersonalAccessTokens
{
    [JsonInclude]
    public string ContinuationToken { get; private set; } = "";

    [JsonInclude]
    [JsonPropertyName("patTokens")]
    public IEnumerable<PersonalAccessToken> PersonalAccessTokens { get; private set; } = Enumerable.Empty<PersonalAccessToken>();
}
