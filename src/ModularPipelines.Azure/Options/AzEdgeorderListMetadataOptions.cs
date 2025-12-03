using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edgeorder", "list-metadata")]
public record AzEdgeorderListMetadataOptions : AzOptions
{
    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }
}