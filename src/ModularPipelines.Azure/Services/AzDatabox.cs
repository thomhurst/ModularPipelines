using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDatabox
{
    public AzDatabox(
        AzDataboxJob job
    )
    {
        Job = job;
    }

    public AzDataboxJob Job { get; }
}