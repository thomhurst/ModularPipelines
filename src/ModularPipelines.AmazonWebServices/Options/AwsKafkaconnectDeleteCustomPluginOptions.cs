using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafkaconnect", "delete-custom-plugin")]
public record AwsKafkaconnectDeleteCustomPluginOptions(
[property: CliOption("--custom-plugin-arn")] string CustomPluginArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}