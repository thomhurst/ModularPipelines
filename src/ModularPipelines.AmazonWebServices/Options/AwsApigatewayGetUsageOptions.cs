using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-usage")]
public record AwsApigatewayGetUsageOptions(
[property: CliOption("--usage-plan-id")] string UsagePlanId,
[property: CliOption("--start-date")] string StartDate,
[property: CliOption("--end-date")] string EndDate
) : AwsOptions
{
    [CliOption("--key-id")]
    public string? KeyId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}