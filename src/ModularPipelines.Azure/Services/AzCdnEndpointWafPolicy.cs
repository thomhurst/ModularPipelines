using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cdn", "endpoint", "waf")]
public class AzCdnEndpointWafPolicy
{
    public AzCdnEndpointWafPolicy(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Remove(AzCdnEndpointWafPolicyRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnEndpointWafPolicyRemoveOptions(), token);
    }

    public async Task<CommandResult> Set(AzCdnEndpointWafPolicySetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnEndpointWafPolicySetOptions(), token);
    }

    public async Task<CommandResult> Show(AzCdnEndpointWafPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCdnEndpointWafPolicyShowOptions(), token);
    }
}