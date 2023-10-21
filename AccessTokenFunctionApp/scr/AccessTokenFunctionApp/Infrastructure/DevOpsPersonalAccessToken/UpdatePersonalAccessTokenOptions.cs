namespace AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;

public class UpdatePersonalAccessTokenOptions
{
    public bool AllOrgs { get; private set; }
    public string AuthorizationId { get; private set; }
    public string DisplayName { get; private set; }
    public string Scope { get; private set; }
    public string ValidTo { get; private set; }

    public UpdatePersonalAccessTokenOptions(bool allOrgs, string authorizationId, string displayName, string scope, string validTo)
    {
        AllOrgs = allOrgs;
        AuthorizationId = authorizationId;
        DisplayName = displayName;
        Scope = scope;
        ValidTo = validTo;
    }

}
