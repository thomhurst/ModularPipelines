using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "revoke-flow-entitlement")]
public record AwsMediaconnectRevokeFlowEntitlementOptions(
[property: CliOption("--entitlement-arn")] string EntitlementArn,
[property: CliOption("--flow-arn")] string FlowArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}