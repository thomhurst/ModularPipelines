using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-quotas", "get-service-quota-increase-request-from-template")]
public record AwsServiceQuotasGetServiceQuotaIncreaseRequestFromTemplateOptions(
[property: CommandSwitch("--service-code")] string ServiceCode,
[property: CommandSwitch("--quota-code")] string QuotaCode,
[property: CommandSwitch("--aws-region")] string AwsRegion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}