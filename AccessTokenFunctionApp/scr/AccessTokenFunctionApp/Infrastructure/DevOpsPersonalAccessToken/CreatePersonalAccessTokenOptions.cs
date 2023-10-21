namespace AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;

public class CreatePersonalAccessTokenOptions
{
    public bool AllOrgs { get; private set; }
    public string DisplayName { get; private set; }
    public string Scope { get; private set; }
    public string ValidTo { get; private set; }
    public CreatePersonalAccessTokenOptions(bool allOrgs, string displayName, string scope, string validTo)
    {
        AllOrgs = allOrgs;
        DisplayName = displayName;
        Scope = scope;
        ValidTo = validTo;
    }
}
