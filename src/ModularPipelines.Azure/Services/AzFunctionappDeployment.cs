using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp")]
public class AzFunctionappDeployment
{
    public AzFunctionappDeployment(
        AzFunctionappDeploymentContainer container,
        AzFunctionappDeploymentGithubActions githubActions,
        AzFunctionappDeploymentSlot slot,
        AzFunctionappDeploymentSource source,
        AzFunctionappDeploymentUser user,
        ICommand internalCommand
    )
    {
        Container = container;
        GithubActions = githubActions;
        Slot = slot;
        Source = source;
        User = user;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzFunctionappDeploymentContainer Container { get; }

    public AzFunctionappDeploymentGithubActions GithubActions { get; }

    public AzFunctionappDeploymentSlot Slot { get; }

    public AzFunctionappDeploymentSource Source { get; }

    public AzFunctionappDeploymentUser User { get; }

    public async Task<CommandResult> ListPublishingCredentials(AzFunctionappDeploymentListPublishingCredentialsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeploymentListPublishingCredentialsOptions(), token);
    }

    public async Task<CommandResult> ListPublishingProfiles(AzFunctionappDeploymentListPublishingProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeploymentListPublishingProfilesOptions(), token);
    }
}