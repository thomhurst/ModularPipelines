using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud", "show")]
public record AzCloudShowOptions : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }
}