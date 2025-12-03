using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("vmware")]
public class GcloudVmwareNetworkPolicies
{
    public GcloudVmwareNetworkPolicies(
        GcloudVmwareNetworkPoliciesExternalAccessRules externalAccessRules,
        ICommand internalCommand
    )
    {
        ExternalAccessRules = externalAccessRules;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudVmwareNetworkPoliciesExternalAccessRules ExternalAccessRules { get; }

    public async Task<CommandResult> Create(GcloudVmwareNetworkPoliciesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudVmwareNetworkPoliciesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudVmwareNetworkPoliciesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudVmwareNetworkPoliciesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudVmwareNetworkPoliciesListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudVmwareNetworkPoliciesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}