using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch")]
public class AzBatchLocation
{
    public AzBatchLocation(
        AzBatchLocationQuotas quotas,
        ICommand internalCommand
    )
    {
        Quotas = quotas;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBatchLocationQuotas Quotas { get; }

    public async Task<CommandResult> ListSkus(AzBatchLocationListSkusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}