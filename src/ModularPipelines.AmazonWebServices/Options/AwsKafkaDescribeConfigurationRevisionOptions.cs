using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "describe-configuration-revision")]
public record AwsKafkaDescribeConfigurationRevisionOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--revision")] long Revision
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}