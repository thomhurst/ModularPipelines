using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "update-usage-limit")]
public record AwsRedshiftServerlessUpdateUsageLimitOptions(
[property: CommandSwitch("--usage-limit-id")] string UsageLimitId
) : AwsOptions
{
    [CommandSwitch("--amount")]
    public long? Amount { get; set; }

    [CommandSwitch("--breach-action")]
    public string? BreachAction { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}