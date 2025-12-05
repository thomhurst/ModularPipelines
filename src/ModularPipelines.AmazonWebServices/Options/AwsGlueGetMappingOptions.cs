using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-mapping")]
public record AwsGlueGetMappingOptions(
[property: CliOption("--source")] string Source
) : AwsOptions
{
    [CliOption("--sinks")]
    public string[]? Sinks { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}