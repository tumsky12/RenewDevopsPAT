namespace AccessTokenFunctionApp.UnitTests.AccessTokenFunctionApp.Infrastructure.Helpers;

internal static class DevopsPatHttpResponseTestHelper
{
    private const string ValidPersonalAccessTokenResultsJson = 
        @"{
            ""patToken"":
            {
                ""displayName"":""Auto Renewal Pat - Exp: 24/11/2023"",
                ""validTo"":""2023-11-24T00:00:00Z"",
                ""scope"":""app_token"",
                ""targetAccounts"":[],
                ""validFrom"":""2023-10-25T19:56:59.37Z"",
                ""authorizationId"":""5a9f2364-02bb-4bd7-97a4-f78a353b2846"",
                ""token"":null
            },
            ""patTokenError"":""none""
        }";

    // private const string ValidPagedPersonalAccessTokensResultsJson = 
    //     @"{
    //         ""continuationToken"":"""",
    //         ""patTokens"":[
    //             {
    //                 ""displayName"":""Auto Renewal Pat - Exp: 24/11/2023"",
    //                 ""validTo"":""2023-11-24T00:00:00Z"",
    //                 ""scope"":""app_token"",
    //                 ""targetAccounts"":[],
    //                 ""validFrom"":""2023-10-25T19:56:59.37Z"",
    //                 ""authorizationId"":""5a9f2364-02bb-4bd7-97a4-f78a353b2846"",
    //                 ""token"":null
    //             },
    //             {
    //                 ""displayName"":""Auto Renewal Pat - Exp: 24/11/2023"",
    //                 ""validTo"":""2023-11-24T00:00:00Z"",
    //                 ""scope"":""app_token"",""targetAccounts"":[],
    //                 ""validFrom"":""2023-10-24T23:01:37.7466667Z"",
    //                 ""authorizationId"":""2be7c030-7270-4674-af10-b67eb1a67331"",
    //                 ""token"":null
    //             }
    //         ]";

    internal static string GetValidPersonalAccessTokenResultsJson()
    {
        return ValidPersonalAccessTokenResultsJson;
    }
    //
    // internal static string GetValidPagedPersonalAccessTokensResultsJson()
    // {
    //     return ValidPagedPersonalAccessTokensResultsJson;
    // }

}
