using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-quotas", "list-service-quotas")]
public record AwsServiceQuotasListServiceQuotasOptions(
[property: CliOption("--service-code")] string ServiceCode
) : AwsOptions
{
    [CliOption("--quota-code")]
    public string? QuotaCode { get; set; }

    [CliOption("--quota-applied-at-level")]
    public string? QuotaAppliedAtLevel { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}