using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("webapp")]
public class AzWebappDeployment
{
    public AzWebappDeployment(
        AzWebappDeploymentContainer container,
        AzWebappDeploymentGithubActions githubActions,
        AzWebappDeploymentSlot slot,
        AzWebappDeploymentSource source,
        AzWebappDeploymentUser user,
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

    public AzWebappDeploymentContainer Container { get; }

    public AzWebappDeploymentGithubActions GithubActions { get; }

    public AzWebappDeploymentSlot Slot { get; }

    public AzWebappDeploymentSource Source { get; }

    public AzWebappDeploymentUser User { get; }

    public async Task<CommandResult> ListPublishingCredentials(AzWebappDeploymentListPublishingCredentialsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappDeploymentListPublishingCredentialsOptions(), token);
    }

    public async Task<CommandResult> ListPublishingProfiles(AzWebappDeploymentListPublishingProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappDeploymentListPublishingProfilesOptions(), token);
    }
}