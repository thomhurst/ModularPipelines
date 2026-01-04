using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("route53")]
public class AwsRoute53Wait
{
    public AwsRoute53Wait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ResourceRecordSetsChanged(AwsRoute53WaitResourceRecordSetsChangedOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}