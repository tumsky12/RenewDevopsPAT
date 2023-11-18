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
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

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
        _logger.LogInformation("response {response}", (await response.Content.ReadAsStringAsync()));
        response.EnsureSuccessStatusCode();
        return await DeserializeResponse<TResponse>(response, cancellationToken);
    }

    private async Task<T> DeserializeResponse<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var deserializedResponse = JsonSerializer.Deserialize<T>(responseBody, _responseDeserializerOptions);
        return deserializedResponse ?? throw new InvalidOperationException();
    }
}