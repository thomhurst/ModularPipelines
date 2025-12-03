using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-quotas", "get--default-service-quota")]
public record AwsServiceQuotasGetAwsDefaultServiceQuotaOptions(
[property: CliOption("--service-code")] string ServiceCode,
[property: CliOption("--quota-code")] string QuotaCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}