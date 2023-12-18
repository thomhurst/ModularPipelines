using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzWorkloads
{
    public AzWorkloads(
        AzWorkloadsMonitor monitor,
        AzWorkloadsSapApplicationServerInstance sapApplicationServerInstance,
        AzWorkloadsSapCentralInstance sapCentralInstance,
        AzWorkloadsSapDatabaseInstance sapDatabaseInstance,
        AzWorkloadsSapVirtualInstance sapVirtualInstance,
        ICommand internalCommand
    )
    {
        Monitor = monitor;
        SapApplicationServerInstance = sapApplicationServerInstance;
        SapCentralInstance = sapCentralInstance;
        SapDatabaseInstance = sapDatabaseInstance;
        SapVirtualInstance = sapVirtualInstance;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzWorkloadsMonitor Monitor { get; }

    public AzWorkloadsSapApplicationServerInstance SapApplicationServerInstance { get; }

    public AzWorkloadsSapCentralInstance SapCentralInstance { get; }

    public AzWorkloadsSapDatabaseInstance SapDatabaseInstance { get; }

    public AzWorkloadsSapVirtualInstance SapVirtualInstance { get; }

    public async Task<CommandResult> SapDiskConfiguration(AzWorkloadsSapDiskConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapDiskConfigurationOptions(), token);
    }

    public async Task<CommandResult> SapSizingRecommendation(AzWorkloadsSapSizingRecommendationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapSizingRecommendationOptions(), token);
    }

    public async Task<CommandResult> SapSupportedSku(AzWorkloadsSapSupportedSkuOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsSapSupportedSkuOptions(), token);
    }
}