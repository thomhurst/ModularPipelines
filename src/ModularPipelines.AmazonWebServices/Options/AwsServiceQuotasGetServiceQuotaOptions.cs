using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-quotas", "get-service-quota")]
public record AwsServiceQuotasGetServiceQuotaOptions(
[property: CommandSwitch("--service-code")] string ServiceCode,
[property: CommandSwitch("--quota-code")] string QuotaCode
) : AwsOptions
{
    [CommandSwitch("--context-id")]
    public string? ContextId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}