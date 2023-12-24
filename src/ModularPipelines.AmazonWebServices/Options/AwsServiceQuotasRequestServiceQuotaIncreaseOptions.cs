using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-quotas", "request-service-quota-increase")]
public record AwsServiceQuotasRequestServiceQuotaIncreaseOptions(
[property: CommandSwitch("--service-code")] string ServiceCode,
[property: CommandSwitch("--quota-code")] string QuotaCode,
[property: CommandSwitch("--desired-value")] double DesiredValue
) : AwsOptions
{
    [CommandSwitch("--context-id")]
    public string? ContextId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}