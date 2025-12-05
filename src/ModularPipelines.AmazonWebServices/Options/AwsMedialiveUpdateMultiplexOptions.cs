using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "update-multiplex")]
public record AwsMedialiveUpdateMultiplexOptions(
[property: CliOption("--multiplex-id")] string MultiplexId
) : AwsOptions
{
    [CliOption("--multiplex-settings")]
    public string? MultiplexSettings { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}