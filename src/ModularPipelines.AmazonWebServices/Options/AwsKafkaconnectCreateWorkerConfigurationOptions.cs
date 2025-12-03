using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafkaconnect", "create-worker-configuration")]
public record AwsKafkaconnectCreateWorkerConfigurationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--properties-file-content")] string PropertiesFileContent
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}