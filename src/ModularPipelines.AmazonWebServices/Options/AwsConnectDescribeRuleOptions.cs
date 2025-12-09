using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "describe-rule")]
public record AwsConnectDescribeRuleOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--rule-id")] string RuleId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}