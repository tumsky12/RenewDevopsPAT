using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;
using AccessTokenFunctionApp.Infrastructure.Interfaces;

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
    public DevOpsPatApiWrapper(string organization, string token)
    {
        _patApiUriBase = $"{DevOpsUrl}/{organization}/{PatTokenApiPath}";
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<PersonalAccessTokenResults> CreateAsync(CreatePersonalAccessTokenOptions patTokenOptions, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_patApiUriBase}?api-version={ApiVersion}";
        var optionsJson = JsonSerializer.Serialize(patTokenOptions, _requestSerializerOptions);
        var optionsContent = new StringContent(optionsJson, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(requestUri, optionsContent, cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        var patToken = JsonSerializer.Deserialize<PersonalAccessTokenResults>(responseBody, _responseDeserializerOptions);
        return patToken ?? throw new InvalidOperationException();
    }

    public async Task<PersonalAccessTokenResults> GetAsync(string authorizationId, CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_patApiUriBase}?authorizationId={authorizationId}&api-version={ApiVersion}";
        var response = await GetResponseAsync(requestUri, cancellationToken);
        var patToken = JsonSerializer.Deserialize<PersonalAccessTokenResults>(response, _responseDeserializerOptions);
        return patToken ?? throw new InvalidOperationException();
    }

    public async Task<PagedPersonalAccessTokens> ListAsync(CancellationToken cancellationToken = default)
    {
        var requestUri = $"{_patApiUriBase}?api-version={ApiVersion}";
        var response = await GetResponseAsync(requestUri, cancellationToken);
        var pagedPatTokens = JsonSerializer.Deserialize<PagedPersonalAccessTokens>(response, _responseDeserializerOptions);
        return pagedPatTokens ?? throw new InvalidOperationException();
    }

    public async Task RevokeAsync(string authorizationId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<PersonalAccessTokenResults> UpdateAsync(UpdatePersonalAccessTokenOptions patTokenOptions, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private async Task<string> GetResponseAsync(string requestUri, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        return responseBody;
    }
}