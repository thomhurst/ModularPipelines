using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service-quotas", "get-requested-service-quota-change")]
public record AwsServiceQuotasGetRequestedServiceQuotaChangeOptions(
[property: CommandSwitch("--request-id")] string RequestId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}