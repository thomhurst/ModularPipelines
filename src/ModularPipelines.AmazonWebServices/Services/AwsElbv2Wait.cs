using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2")]
public class AwsElbv2Wait
{
    public AwsElbv2Wait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> LoadBalancerAvailable(AwsElbv2WaitLoadBalancerAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2WaitLoadBalancerAvailableOptions(), token);
    }

    public async Task<CommandResult> LoadBalancerExists(AwsElbv2WaitLoadBalancerExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2WaitLoadBalancerExistsOptions(), token);
    }

    public async Task<CommandResult> LoadBalancersDeleted(AwsElbv2WaitLoadBalancersDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsElbv2WaitLoadBalancersDeletedOptions(), token);
    }

    public async Task<CommandResult> TargetDeregistered(AwsElbv2WaitTargetDeregisteredOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TargetInService(AwsElbv2WaitTargetInServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}