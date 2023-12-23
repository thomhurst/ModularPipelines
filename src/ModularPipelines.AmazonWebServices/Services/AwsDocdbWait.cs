using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb")]
public class AwsDocdbWait
{
    public AwsDocdbWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DbInstanceAvailable(AwsDocdbWaitDbInstanceAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbWaitDbInstanceAvailableOptions(), token);
    }

    public async Task<CommandResult> DbInstanceDeleted(AwsDocdbWaitDbInstanceDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbWaitDbInstanceDeletedOptions(), token);
    }
}