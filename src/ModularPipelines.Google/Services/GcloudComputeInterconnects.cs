using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute")]
public class GcloudComputeInterconnects
{
    public GcloudComputeInterconnects(
        GcloudComputeInterconnectsAttachments attachments,
        GcloudComputeInterconnectsLocations locations,
        GcloudComputeInterconnectsMacsec macsec,
        GcloudComputeInterconnectsRemoteLocations remoteLocations,
        ICommand internalCommand
    )
    {
        Attachments = attachments;
        Locations = locations;
        Macsec = macsec;
        RemoteLocations = remoteLocations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeInterconnectsAttachments Attachments { get; }

    public GcloudComputeInterconnectsLocations Locations { get; }

    public GcloudComputeInterconnectsMacsec Macsec { get; }

    public GcloudComputeInterconnectsRemoteLocations RemoteLocations { get; }

    public async Task<CommandResult> Create(GcloudComputeInterconnectsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudComputeInterconnectsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComputeInterconnectsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDiagnostics(GcloudComputeInterconnectsGetDiagnosticsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputeInterconnectsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeInterconnectsListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudComputeInterconnectsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}