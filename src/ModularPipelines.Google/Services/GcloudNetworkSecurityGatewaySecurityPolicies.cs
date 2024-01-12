using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security")]
public class GcloudNetworkSecurityGatewaySecurityPolicies
{
    public GcloudNetworkSecurityGatewaySecurityPolicies(
        GcloudNetworkSecurityGatewaySecurityPoliciesRules rules,
        ICommand internalCommand
    )
    {
        Rules = rules;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudNetworkSecurityGatewaySecurityPoliciesRules Rules { get; }

    public async Task<CommandResult> Delete(GcloudNetworkSecurityGatewaySecurityPoliciesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Export(GcloudNetworkSecurityGatewaySecurityPoliciesExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Import(GcloudNetworkSecurityGatewaySecurityPoliciesImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudNetworkSecurityGatewaySecurityPoliciesListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}