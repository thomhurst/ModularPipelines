using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "job")]
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