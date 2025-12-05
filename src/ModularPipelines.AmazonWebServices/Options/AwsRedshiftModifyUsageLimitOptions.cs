using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-usage-limit")]
public record AwsRedshiftModifyUsageLimitOptions(
[property: CliOption("--usage-limit-id")] string UsageLimitId
) : AwsOptions
{
    [CliOption("--amount")]
    public long? Amount { get; set; }

    [CliOption("--breach-action")]
    public string? BreachAction { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}