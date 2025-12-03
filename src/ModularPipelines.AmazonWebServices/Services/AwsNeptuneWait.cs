using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliCommand("neptune")]
public class AwsNeptuneWait
{
    public AwsNeptuneWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DbInstanceAvailable(AwsNeptuneWaitDbInstanceAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneWaitDbInstanceAvailableOptions(), token);
    }

    public async Task<CommandResult> DbInstanceDeleted(AwsNeptuneWaitDbInstanceDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsNeptuneWaitDbInstanceDeletedOptions(), token);
    }
}