using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafkaconnect", "describe-custom-plugin")]
public record AwsKafkaconnectDescribeCustomPluginOptions(
[property: CliOption("--custom-plugin-arn")] string CustomPluginArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}