using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "create-rule")]
public record AwsElbv2CreateRuleOptions(
[property: CliOption("--listener-arn")] string ListenerArn,
[property: CliOption("--conditions")] string[] Conditions,
[property: CliOption("--priority")] int Priority,
[property: CliOption("--actions")] string[] Actions
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}