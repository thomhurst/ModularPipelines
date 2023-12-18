using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "account")]
public class AzBatchAccountNetworkProfile
{
    public AzBatchAccountNetworkProfile(
        AzBatchAccountNetworkProfileNetworkRule networkRule,
        ICommand internalCommand
    )
    {
        NetworkRule = networkRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBatchAccountNetworkProfileNetworkRule NetworkRule { get; }

    public async Task<CommandResult> Set(AzBatchAccountNetworkProfileSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBatchAccountNetworkProfileShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}