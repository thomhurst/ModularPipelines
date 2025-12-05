using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-indexing-configuration")]
public record AwsIotUpdateIndexingConfigurationOptions : AwsOptions
{
    [CliOption("--thing-indexing-configuration")]
    public string? ThingIndexingConfiguration { get; set; }

    [CliOption("--thing-group-indexing-configuration")]
    public string? ThingGroupIndexingConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}