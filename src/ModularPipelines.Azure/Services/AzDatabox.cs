using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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