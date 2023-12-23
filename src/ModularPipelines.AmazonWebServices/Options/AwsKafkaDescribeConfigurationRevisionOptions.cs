using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kafka", "describe-configuration-revision")]
public record AwsKafkaDescribeConfigurationRevisionOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--revision")] long Revision
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}