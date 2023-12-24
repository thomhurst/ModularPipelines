using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "describe-account-policies")]
public record AwsLogsDescribeAccountPoliciesOptions(
[property: CommandSwitch("--policy-type")] string PolicyType
) : AwsOptions
{
    [CommandSwitch("--policy-name")]
    public string? PolicyName { get; set; }

    [CommandSwitch("--account-identifiers")]
    public string[]? AccountIdentifiers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}