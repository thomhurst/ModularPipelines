using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataproc")]
public class GcloudDataprocBatches
{
    public GcloudDataprocBatches(
        GcloudDataprocBatchesSubmit submit,
        ICommand internalCommand
    )
    {
        Submit = submit;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDataprocBatchesSubmit Submit { get; }

    public async Task<CommandResult> Cancel(GcloudDataprocBatchesCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudDataprocBatchesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudDataprocBatchesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudDataprocBatchesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudDataprocBatchesListOptions(), token);
    }

    public async Task<CommandResult> Wait(GcloudDataprocBatchesWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}