using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet")]
public class GcloudContainerFleetMemberships
{
    public GcloudContainerFleetMemberships(
        GcloudContainerFleetMembershipsBindings bindings,
        ICommand internalCommand
    )
    {
        Bindings = bindings;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerFleetMembershipsBindings Bindings { get; }

    public async Task<CommandResult> Delete(GcloudContainerFleetMembershipsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudContainerFleetMembershipsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateGatewayRbac(GcloudContainerFleetMembershipsGenerateGatewayRbacOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCredentials(GcloudContainerFleetMembershipsGetCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudContainerFleetMembershipsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetMembershipsListOptions(), token);
    }

    public async Task<CommandResult> Register(GcloudContainerFleetMembershipsRegisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Unregister(GcloudContainerFleetMembershipsUnregisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudContainerFleetMembershipsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}