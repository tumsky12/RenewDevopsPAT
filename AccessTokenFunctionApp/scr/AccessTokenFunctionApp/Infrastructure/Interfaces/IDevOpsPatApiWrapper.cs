using System.Threading;
using System.Threading.Tasks;
using AccessTokenFunctionApp.Infrastructure.DevOpsPersonalAccessToken;

namespace AccessTokenFunctionApp.Infrastructure.Interfaces;

public interface IDevOpsPatApiWrapper
{
    Task<PersonalAccessTokenResults> CreateAsync(CreatePersonalAccessTokenOptions patTokenOptions, CancellationToken cancellationToken = default);
    Task<PersonalAccessTokenResults> GetAsync(string authorizationId, CancellationToken cancellationToken = default);
    Task<PagedPersonalAccessTokens> ListAsync(CancellationToken cancellationToken = default);
    Task RevokeAsync(string authorizationId, CancellationToken cancellationToken = default);
    Task<PersonalAccessTokenResults> UpdateAsync(UpdatePersonalAccessTokenOptions patTokenOptions, CancellationToken cancellationToken = default);
}