using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;

public class PersonalAccessToken
{
    [JsonInclude]
    public string? AuthorizationId { get; private set; }

    [JsonInclude]
    public string? DisplayName { get; private set; }

    [JsonInclude]
    public string? Scope { get; private set; }

    [JsonInclude]
    public IEnumerable<string> TargetAccounts { get; private set; } = Enumerable.Empty<string>();

    [JsonInclude]
    public string? Token { get; private set; }

    [JsonInclude]
    public string? ValidFrom { get; private set; }

    [JsonInclude]
    public string? ValidTo { get; private set; }
}
