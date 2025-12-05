using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "delete-usage-plan-key")]
public record AwsApigatewayDeleteUsagePlanKeyOptions(
[property: CliOption("--usage-plan-id")] string UsagePlanId,
[property: CliOption("--key-id")] string KeyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}