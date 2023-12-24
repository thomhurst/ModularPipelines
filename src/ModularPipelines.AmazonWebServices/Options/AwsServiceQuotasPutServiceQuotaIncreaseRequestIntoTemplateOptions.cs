using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-quotas", "put-service-quota-increase-request-into-template")]
public record AwsServiceQuotasPutServiceQuotaIncreaseRequestIntoTemplateOptions(
[property: CommandSwitch("--quota-code")] string QuotaCode,
[property: CommandSwitch("--service-code")] string ServiceCode,
[property: CommandSwitch("--aws-region")] string AwsRegion,
[property: CommandSwitch("--desired-value")] double DesiredValue
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}