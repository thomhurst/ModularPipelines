using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-usage-limit")]
public record AwsRedshiftCreateUsageLimitOptions(
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier,
[property: CommandSwitch("--feature-type")] string FeatureType,
[property: CommandSwitch("--limit-type")] string LimitType,
[property: CommandSwitch("--amount")] long Amount
) : AwsOptions
{
    [CommandSwitch("--period")]
    public string? Period { get; set; }

    [CommandSwitch("--breach-action")]
    public string? BreachAction { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}