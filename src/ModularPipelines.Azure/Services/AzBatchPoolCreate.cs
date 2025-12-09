using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "pool")]
public class AzBatchPoolCreate
{
    public AzBatchPoolCreate(
        AzBatchPoolCreateAzureBatchCliExtensions azureBatchCliExtensions
    )
    {
        AzureBatchCliExtensions = azureBatchCliExtensions;
    }

    public AzBatchPoolCreateAzureBatchCliExtensions AzureBatchCliExtensions { get; }
}