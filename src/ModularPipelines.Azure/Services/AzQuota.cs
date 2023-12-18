using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzQuota
{
    public AzQuota(
        AzQuotaOperation operation,
        AzQuotaRequest request,
        AzQuotaUsage usage,
        ICommand internalCommand
    )
    {
        Operation = operation;
        Request = request;
        Usage = usage;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzQuotaOperation Operation { get; }

    public AzQuotaRequest Request { get; }

    public AzQuotaUsage Usage { get; }

    public async Task<CommandResult> Create(AzQuotaCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzQuotaListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzQuotaShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzQuotaUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}