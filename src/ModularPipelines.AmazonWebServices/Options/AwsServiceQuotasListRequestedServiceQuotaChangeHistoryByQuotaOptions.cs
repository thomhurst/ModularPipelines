using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-quotas", "list-requested-service-quota-change-history-by-quota")]
public record AwsServiceQuotasListRequestedServiceQuotaChangeHistoryByQuotaOptions(
[property: CommandSwitch("--service-code")] string ServiceCode,
[property: CommandSwitch("--quota-code")] string QuotaCode
) : AwsOptions
{
    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--quota-requested-at-level")]
    public string? QuotaRequestedAtLevel { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}