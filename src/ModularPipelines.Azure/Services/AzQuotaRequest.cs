using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quota")]
public class AzQuotaRequest
{
    public AzQuotaRequest(
        AzQuotaRequestStatus status,
        ICommand internalCommand
    )
    {
        Status = status;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzQuotaRequestStatus Status { get; }

    public async Task<CommandResult> List(AzQuotaRequestListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzQuotaRequestShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}