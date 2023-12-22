using System.Diagnostics.CodeAnalysis;

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