using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-quotas", "delete-service-quota-increase-request-from-template")]
public record AwsServiceQuotasDeleteServiceQuotaIncreaseRequestFromTemplateOptions(
[property: CliOption("--service-code")] string ServiceCode,
[property: CliOption("--quota-code")] string QuotaCode,
[property: CliOption("--aws-region")] string AwsRegion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}