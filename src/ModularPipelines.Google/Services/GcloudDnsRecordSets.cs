using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("dns")]
public class GcloudDnsRecordSets
{
    public GcloudDnsRecordSets(
        GcloudDnsRecordSetsChanges changes,
        GcloudDnsRecordSetsTransaction transaction,
        ICommand internalCommand
    )
    {
        Changes = changes;
        Transaction = transaction;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDnsRecordSetsChanges Changes { get; }

    public GcloudDnsRecordSetsTransaction Transaction { get; }

    public async Task<CommandResult> Create(GcloudDnsRecordSetsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudDnsRecordSetsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudDnsRecordSetsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Export(GcloudDnsRecordSetsExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(GcloudDnsRecordSetsImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudDnsRecordSetsListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudDnsRecordSetsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}