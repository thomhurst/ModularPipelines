using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> GetOrganizationRecommendation(AwsTrustedadvisorGetOrganizationRecommendationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetRecommendation(AwsTrustedadvisorGetRecommendationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListChecks(AwsTrustedadvisorListChecksOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTrustedadvisorListChecksOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListOrganizationRecommendationAccounts(AwsTrustedadvisorListOrganizationRecommendationAccountsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListOrganizationRecommendationResources(AwsTrustedadvisorListOrganizationRecommendationResourcesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListOrganizationRecommendations(AwsTrustedadvisorListOrganizationRecommendationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTrustedadvisorListOrganizationRecommendationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListRecommendationResources(AwsTrustedadvisorListRecommendationResourcesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListRecommendations(AwsTrustedadvisorListRecommendationsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTrustedadvisorListRecommendationsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateOrganizationRecommendationLifecycle(AwsTrustedadvisorUpdateOrganizationRecommendationLifecycleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateRecommendationLifecycle(AwsTrustedadvisorUpdateRecommendationLifecycleOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}