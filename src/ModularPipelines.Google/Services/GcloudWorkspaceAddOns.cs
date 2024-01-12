using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudWorkspaceAddOns
{
    public GcloudWorkspaceAddOns(
        GcloudWorkspaceAddOnsDeployments deployments,
        ICommand internalCommand
    )
    {
        Deployments = deployments;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudWorkspaceAddOnsDeployments Deployments { get; }

    public async Task<CommandResult> GetAuthorization(GcloudWorkspaceAddOnsGetAuthorizationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudWorkspaceAddOnsGetAuthorizationOptions(), token);
    }
}