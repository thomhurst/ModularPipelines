using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-quotas", "list-service-quotas")]
public record AwsServiceQuotasListServiceQuotasOptions(
[property: CommandSwitch("--service-code")] string ServiceCode
) : AwsOptions
{
    [CommandSwitch("--quota-code")]
    public string? QuotaCode { get; set; }

    [CommandSwitch("--quota-applied-at-level")]
    public string? QuotaAppliedAtLevel { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}