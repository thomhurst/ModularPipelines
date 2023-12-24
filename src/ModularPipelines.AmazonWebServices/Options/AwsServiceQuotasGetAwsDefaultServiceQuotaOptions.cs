using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-quotas", "get--default-service-quota")]
public record AwsServiceQuotasGetAwsDefaultServiceQuotaOptions(
[property: CommandSwitch("--service-code")] string ServiceCode,
[property: CommandSwitch("--quota-code")] string QuotaCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}