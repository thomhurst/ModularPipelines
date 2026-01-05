using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("elbv2")]
public class AwsElbv2Wait
{
    public AwsElbv2Wait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> LoadBalancerAvailable(AwsElbv2WaitLoadBalancerAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2WaitLoadBalancerAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> LoadBalancerExists(AwsElbv2WaitLoadBalancerExistsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2WaitLoadBalancerExistsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> LoadBalancersDeleted(AwsElbv2WaitLoadBalancersDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2WaitLoadBalancersDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TargetDeregistered(AwsElbv2WaitTargetDeregisteredOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TargetInService(AwsElbv2WaitTargetInServiceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}