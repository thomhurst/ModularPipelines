using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "endpoint", "waf")]
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