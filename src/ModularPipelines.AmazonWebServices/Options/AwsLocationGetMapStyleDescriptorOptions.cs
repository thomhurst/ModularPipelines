using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "get-map-style-descriptor")]
public record AwsLocationGetMapStyleDescriptorOptions(
[property: CliOption("--map-name")] string MapName
) : AwsOptions
{
    [CliOption("--key")]
    public string? Key { get; set; }
}