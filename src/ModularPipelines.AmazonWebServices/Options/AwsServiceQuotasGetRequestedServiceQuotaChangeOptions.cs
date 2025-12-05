using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-quotas", "get-requested-service-quota-change")]
public record AwsServiceQuotasGetRequestedServiceQuotaChangeOptions(
[property: CliOption("--request-id")] string RequestId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}