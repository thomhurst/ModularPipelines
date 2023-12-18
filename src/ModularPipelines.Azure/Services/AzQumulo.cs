using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzQumulo
{
    public AzQumulo(
        AzQumuloStorage storage
    )
    {
        Storage = storage;
    }

    public AzQumuloStorage Storage { get; }
}