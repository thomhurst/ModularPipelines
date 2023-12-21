using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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