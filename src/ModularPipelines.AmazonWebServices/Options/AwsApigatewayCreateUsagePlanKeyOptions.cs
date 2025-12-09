using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "create-usage-plan-key")]
public record AwsApigatewayCreateUsagePlanKeyOptions(
[property: CliOption("--usage-plan-id")] string UsagePlanId,
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--key-type")] string KeyType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}