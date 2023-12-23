using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "create-usage-limit")]
public record AwsRedshiftServerlessCreateUsageLimitOptions(
[property: CommandSwitch("--amount")] long Amount,
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--usage-type")] string UsageType
) : AwsOptions
{
    [CommandSwitch("--breach-action")]
    public string? BreachAction { get; set; }

    [CommandSwitch("--period")]
    public string? Period { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}