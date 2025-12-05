using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "create-configuration")]
public record AwsKafkaCreateConfigurationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--server-properties")] string ServerProperties
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--kafka-versions")]
    public string[]? KafkaVersions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}