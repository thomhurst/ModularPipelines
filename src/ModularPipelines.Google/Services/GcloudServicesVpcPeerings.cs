using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("services")]
public class GcloudServicesVpcPeerings
{
    public GcloudServicesVpcPeerings(
        GcloudServicesVpcPeeringsOperations operations,
        ICommand internalCommand
    )
    {
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudServicesVpcPeeringsOperations Operations { get; }

    public async Task<CommandResult> Connect(GcloudServicesVpcPeeringsConnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudServicesVpcPeeringsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableVpcServiceControls(GcloudServicesVpcPeeringsDisableVpcServiceControlsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableVpcServiceControls(GcloudServicesVpcPeeringsEnableVpcServiceControlsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVpcServiceControls(GcloudServicesVpcPeeringsGetVpcServiceControlsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudServicesVpcPeeringsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudServicesVpcPeeringsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}