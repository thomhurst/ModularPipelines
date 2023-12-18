using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzPowerbi
{
    public AzPowerbi(
        AzPowerbiEmbeddedCapacity embeddedCapacity
    )
    {
        EmbeddedCapacity = embeddedCapacity;
    }

    public AzPowerbiEmbeddedCapacity EmbeddedCapacity { get; }
}