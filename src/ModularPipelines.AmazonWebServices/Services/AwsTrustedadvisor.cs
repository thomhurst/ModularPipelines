using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsTrustedadvisor
{
    public AwsTrustedadvisor(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> GetOrganizationRecommendation(AwsTrustedadvisorGetOrganizationRecommendationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRecommendation(AwsTrustedadvisorGetRecommendationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChecks(AwsTrustedadvisorListChecksOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTrustedadvisorListChecksOptions(), token);
    }

    public async Task<CommandResult> ListOrganizationRecommendationAccounts(AwsTrustedadvisorListOrganizationRecommendationAccountsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListOrganizationRecommendationResources(AwsTrustedadvisorListOrganizationRecommendationResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListOrganizationRecommendations(AwsTrustedadvisorListOrganizationRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTrustedadvisorListOrganizationRecommendationsOptions(), token);
    }

    public async Task<CommandResult> ListRecommendationResources(AwsTrustedadvisorListRecommendationResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRecommendations(AwsTrustedadvisorListRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTrustedadvisorListRecommendationsOptions(), token);
    }

    public async Task<CommandResult> UpdateOrganizationRecommendationLifecycle(AwsTrustedadvisorUpdateOrganizationRecommendationLifecycleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateRecommendationLifecycle(AwsTrustedadvisorUpdateRecommendationLifecycleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}