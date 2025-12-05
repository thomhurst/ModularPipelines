using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "update-usage")]
public record AwsApigatewayUpdateUsageOptions(
[property: CliOption("--usage-plan-id")] string UsagePlanId,
[property: CliOption("--key-id")] string KeyId
) : AwsOptions
{
    [CliOption("--patch-operations")]
    public string[]? PatchOperations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}