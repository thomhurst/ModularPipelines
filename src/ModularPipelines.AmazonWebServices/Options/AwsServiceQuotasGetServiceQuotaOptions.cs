using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-quotas", "get-service-quota")]
public record AwsServiceQuotasGetServiceQuotaOptions(
[property: CliOption("--service-code")] string ServiceCode,
[property: CliOption("--quota-code")] string QuotaCode
) : AwsOptions
{
    [CliOption("--context-id")]
    public string? ContextId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}