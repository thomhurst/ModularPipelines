using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-quotas", "list-requested-service-quota-change-history-by-quota")]
public record AwsServiceQuotasListRequestedServiceQuotaChangeHistoryByQuotaOptions(
[property: CliOption("--service-code")] string ServiceCode,
[property: CliOption("--quota-code")] string QuotaCode
) : AwsOptions
{
    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--quota-requested-at-level")]
    public string? QuotaRequestedAtLevel { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}