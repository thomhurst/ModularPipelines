using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloud", "list-profiles")]
public record AzCloudListProfilesOptions : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--show-all")]
    public bool? ShowAll { get; set; }
}