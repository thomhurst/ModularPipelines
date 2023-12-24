using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "describe-remediation-exceptions")]
public record AwsConfigserviceDescribeRemediationExceptionsOptions(
[property: CommandSwitch("--config-rule-name")] string ConfigRuleName
) : AwsOptions
{
    [CommandSwitch("--resource-keys")]
    public string[]? ResourceKeys { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}