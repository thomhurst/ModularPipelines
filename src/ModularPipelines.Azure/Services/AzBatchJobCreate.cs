using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "job")]
public class AzBatchJobCreate
{
    public AzBatchJobCreate(
        AzBatchJobCreateAzureBatchCliExtensions azureBatchCliExtensions
    )
    {
        AzureBatchCliExtensions = azureBatchCliExtensions;
    }

    public AzBatchJobCreateAzureBatchCliExtensions AzureBatchCliExtensions { get; }
}