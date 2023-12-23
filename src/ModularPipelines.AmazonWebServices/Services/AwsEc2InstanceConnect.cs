using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsEc2InstanceConnect
{
    public AwsEc2InstanceConnect(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> OpenTunnel(AwsEc2InstanceConnectOpenTunnelOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsEc2InstanceConnectOpenTunnelOptions(), token);
    }

    public async Task<CommandResult> SendSerialConsoleSshPublicKey(AwsEc2InstanceConnectSendSerialConsoleSshPublicKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendSshPublicKey(AwsEc2InstanceConnectSendSshPublicKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Ssh(AwsEc2InstanceConnectSshOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}