using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "grant-flow-entitlements")]
public record AwsMediaconnectGrantFlowEntitlementsOptions(
[property: CliOption("--entitlements")] string[] Entitlements,
[property: CliOption("--flow-arn")] string FlowArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}