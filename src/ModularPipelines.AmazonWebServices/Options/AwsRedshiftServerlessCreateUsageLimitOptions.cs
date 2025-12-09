using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "create-usage-limit")]
public record AwsRedshiftServerlessCreateUsageLimitOptions(
[property: CliOption("--amount")] long Amount,
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--usage-type")] string UsageType
) : AwsOptions
{
    [CliOption("--breach-action")]
    public string? BreachAction { get; set; }

    [CliOption("--period")]
    public string? Period { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}