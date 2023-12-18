using System.Diagnostics.CodeAnalysis;

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