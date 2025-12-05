using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("service-quotas", "put-service-quota-increase-request-into-template")]
public record AwsServiceQuotasPutServiceQuotaIncreaseRequestIntoTemplateOptions(
[property: CliOption("--quota-code")] string QuotaCode,
[property: CliOption("--service-code")] string ServiceCode,
[property: CliOption("--aws-region")] string AwsRegion,
[property: CliOption("--desired-value")] double DesiredValue
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}