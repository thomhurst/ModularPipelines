using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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