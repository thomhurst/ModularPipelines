using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("docdb")]
public class AwsDocdbWait
{
    public AwsDocdbWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DbInstanceAvailable(AwsDocdbWaitDbInstanceAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbWaitDbInstanceAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DbInstanceDeleted(AwsDocdbWaitDbInstanceDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDocdbWaitDbInstanceDeletedOptions(), executionOptions, cancellationToken);
    }
}