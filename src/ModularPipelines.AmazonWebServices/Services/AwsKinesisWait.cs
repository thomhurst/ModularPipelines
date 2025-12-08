using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("kinesis")]
public class AwsKinesisWait
{
    public AwsKinesisWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> StreamExists(AwsKinesisWaitStreamExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisWaitStreamExistsOptions(), token);
    }

    public async Task<CommandResult> StreamNotExists(AwsKinesisWaitStreamNotExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsKinesisWaitStreamNotExistsOptions(), token);
    }
}