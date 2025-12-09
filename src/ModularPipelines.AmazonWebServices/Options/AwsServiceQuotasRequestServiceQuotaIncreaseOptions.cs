using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-quotas", "request-service-quota-increase")]
public record AwsServiceQuotasRequestServiceQuotaIncreaseOptions(
[property: CliOption("--service-code")] string ServiceCode,
[property: CliOption("--quota-code")] string QuotaCode,
[property: CliOption("--desired-value")] double DesiredValue
) : AwsOptions
{
    [CliOption("--context-id")]
    public string? ContextId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}