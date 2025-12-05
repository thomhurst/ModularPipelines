using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager")]
public class GcloudCertificateManagerMaps
{
    public GcloudCertificateManagerMaps(
        GcloudCertificateManagerMapsEntries entries,
        ICommand internalCommand
    )
    {
        Entries = entries;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudCertificateManagerMapsEntries Entries { get; }

    public async Task<CommandResult> Create(GcloudCertificateManagerMapsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudCertificateManagerMapsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudCertificateManagerMapsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudCertificateManagerMapsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudCertificateManagerMapsListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudCertificateManagerMapsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}