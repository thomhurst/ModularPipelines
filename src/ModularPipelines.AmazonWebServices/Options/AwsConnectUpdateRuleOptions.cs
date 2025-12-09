using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-rule")]
public record AwsConnectUpdateRuleOptions(
[property: CliOption("--rule-id")] string RuleId,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--function")] string Function,
[property: CliOption("--actions")] string[] Actions,
[property: CliOption("--publish-status")] string PublishStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}