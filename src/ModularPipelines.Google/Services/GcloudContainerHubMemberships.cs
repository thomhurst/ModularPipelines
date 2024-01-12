using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub")]
public class GcloudContainerHubMemberships
{
    public GcloudContainerHubMemberships(
        GcloudContainerHubMembershipsBindings bindings,
        ICommand internalCommand
    )
    {
        Bindings = bindings;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudContainerHubMembershipsBindings Bindings { get; }

    public async Task<CommandResult> Delete(GcloudContainerHubMembershipsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudContainerHubMembershipsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateGatewayRbac(GcloudContainerHubMembershipsGenerateGatewayRbacOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCredentials(GcloudContainerHubMembershipsGetCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudContainerHubMembershipsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubMembershipsListOptions(), token);
    }

    public async Task<CommandResult> Register(GcloudContainerHubMembershipsRegisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Unregister(GcloudContainerHubMembershipsUnregisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudContainerHubMembershipsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}