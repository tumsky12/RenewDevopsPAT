using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;
using AccessTokenFunctionApp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AccessTokenFunctionApp.Infrastructure;

public class DevOpsPatApiWrapper : IDevOpsPatApiWrapper
{
    private const string DevOpsUrl = "https://vssps.dev.azure.com";
    private const string PatTokenApiPath = "_apis/tokens/pats";
    private const string ApiVersion = "7.1-preview.1";

    private readonly JsonSerializerOptions _responseDeserializerOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly JsonSerializerOptions _requestSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private readonly string _patApiUriBase;
    private readonly HttpClient _httpClient;
    private readonly ILogger<DevOpsPatApiWrapper> _logger;

    public DevOpsPatApiWrapper(string bearerToken, IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<DevOpsPatApiWrapper> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSIsImtpZCI6IjlHbW55RlBraGMzaE91UjIybXZTdmduTG83WSJ9.eyJhdWQiOiI0OTliODRhYy0xMzIxLTQyN2YtYWExNy0yNjdjYTY5NzU3OTgiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC9hNTYzOGNkNC1mZDI1LTQwZjEtOTNlZC01ZGViZWYwNjhhYjkvIiwiaWF0IjoxNzAwMzI0MTQ1LCJuYmYiOjE3MDAzMjQxNDUsImV4cCI6MTcwMDMyODUwNCwiYWNyIjoiMSIsImFpbyI6IkFZUUFlLzhWQUFBQWdaYjFzNjZmRFhLalYwWWN0eUxUc1c0WFlmVVo3Y2k2WkRybWVRUnQ2QjVVdEFoUTlMejhxQXRScjdFdDA0dHZhNERQUUtkamE0VEhSUFRhSEd3b2Y1K1lKVStyeEFVdTFaWVBENnJwSjRkNDkvM1NzbDlzVVhpc1Z6eFI1VkVraENGdU1RcUoyandSVUlQWVdjbjRpUVVnemtMNzBoUzBjZzV6aVd1ekw4UT0iLCJhbHRzZWNpZCI6IjE6bGl2ZS5jb206MDAwMzdGRkU0N0RFQkQwNCIsImFtciI6WyJwd2QiXSwiYXBwaWQiOiIwNGIwNzc5NS04ZGRiLTQ2MWEtYmJlZS0wMmY5ZTFiZjdiNDYiLCJhcHBpZGFjciI6IjAiLCJlbWFpbCI6ImF0dW10dW0xMkBnbWFpbC5jb20iLCJmYW1pbHlfbmFtZSI6IkciLCJnaXZlbl9uYW1lIjoiQXJraXVzIiwiaWRwIjoibGl2ZS5jb20iLCJpcGFkZHIiOiI4Ni4xNDkuMTI3LjQwIiwibmFtZSI6IkFya2l1cyIsIm9pZCI6IjIzOTIzNTNhLTZkNmYtNDNiNC1iNDE1LWEwYWE5OGMzOTI5OSIsInB1aWQiOiIxMDAzMjAwMkE3NDg2NzYwIiwicmgiOiIwLkFVNEExSXhqcFNYOThVQ1Q3VjNyN3dhS3VheUVtMGtoRTM5Q3FoY21mS2FYVjVpREFDTS4iLCJzY3AiOiJ1c2VyX2ltcGVyc29uYXRpb24iLCJzdWIiOiJaN1gtMVl6V0hBUmJFNzFSVEVRU1cwazcwTmYxX0FpRWFzTEVmSTRmQWVNIiwidGlkIjoiYTU2MzhjZDQtZmQyNS00MGYxLTkzZWQtNWRlYmVmMDY4YWI5IiwidW5pcXVlX25hbWUiOiJsaXZlLmNvbSNhdHVtdHVtMTJAZ21haWwuY29tIiwidXRpIjoibi02Qk1lTDljVXFNMmhJWW9qcVRBQSIsInZlciI6IjEuMCIsIndpZHMiOlsiNjJlOTAzOTQtNjlmNS00MjM3LTkxOTAtMDEyMTc3MTQ1ZTEwIiwiYjc5ZmJmNGQtM2VmOS00Njg5LTgxNDMtNzZiMTk0ZTg1NTA5Il19.OgtkUG65HCfeW4SdwWdH6mJCFy4T_1f3VEO0Aj1SBwUCwyzsJZFEtf_CvQMj9aEGAuWLHbBANDkX8dTHOPt0dusHqFt2dJhlvIw6L7TtJO2w5ddbnC11V9W98cHeD7pRLF9J5N7vaaOUPZI9NBl0uHoPcr4G7xQhspdC1J1oYqw0tkGFlVK_quKGf3NOfdnRrU8rTqdHEA3xgNQN9pF68MEO2f-ZluFSb2xO24lgE4RK4xHdEhCmOcgSUJjHNp5bmCU1Y9I2REo5NkAb8S-__sZNAfFRX2X8vatjzf-_Nlm8YLIVTlokHOdyBGAfMcsI1NeERSGVUEKcrozRsg7jzA");

        var organization = configuration.GetValue<string>("DEVOPS_ORGANIZATION_NAME") ?? throw new Exception("DEVOPS_ORGANIZATION_NAME configuration variable not found");
        _patApiUriBase = $"{DevOpsUrl}/{organization}/{PatTokenApiPath}";
        
        _logger = logger;
        _logger.LogInformation("bearerToken {bearerToken}", bearerToken);
    }

    public async Task<PersonalAccessTokenResults> CreateAsync(CreatePersonalAccessTokenOptions patTokenOptions, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_patApiUriBase}?api-version={ApiVersion}";
        return await PostRequestAsync<PersonalAccessTokenResults, CreatePersonalAccessTokenOptions>(patTokenOptions, requestUri, cancellationToken);
    }

    public async Task<PersonalAccessTokenResults> GetAsync(string authorizationId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_patApiUriBase}?authorizationId={authorizationId}&api-version={ApiVersion}";
        return await GetRequestAsync<PersonalAccessTokenResults>(requestUri, cancellationToken);
    }

    public async Task<PagedPersonalAccessTokens> ListAsync(CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_patApiUriBase}?api-version={ApiVersion}";
        return await GetRequestAsync<PagedPersonalAccessTokens>(requestUri, cancellationToken);
    }

    public Task RevokeAsync(string authorizationId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PersonalAccessTokenResults> UpdateAsync(UpdatePersonalAccessTokenOptions patTokenOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task<TResponse> GetRequestAsync<TResponse>(string requestUri, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await DeserializeResponse<TResponse>(response, cancellationToken);
    }

    private async Task<TResponse> PostRequestAsync<TResponse, TOptions>(TOptions options, string requestUri, CancellationToken cancellationToken = default)
    {
        var optionsJson = JsonSerializer.Serialize(options, _requestSerializerOptions);
        var optionsContent = new StringContent(optionsJson, Encoding.UTF8, "application/json");
        _logger.LogInformation("requestUri {requestUri}", requestUri);
        _logger.LogInformation("optionsJson {optionsJson}", optionsJson);
        var response = await _httpClient.PostAsync(requestUri, optionsContent, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogInformation("content {content}", content);
        //response.EnsureSuccessStatusCode();
        return await DeserializeResponse<TResponse>(response, cancellationToken);
    }

    private async Task<T> DeserializeResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var deserializedResponse = JsonSerializer.Deserialize<T>(responseBody, _responseDeserializerOptions);
        return deserializedResponse ?? throw new InvalidOperationException();
    }
}