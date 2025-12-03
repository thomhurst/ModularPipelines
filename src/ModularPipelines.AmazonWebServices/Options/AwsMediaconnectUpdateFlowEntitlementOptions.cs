using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "update-flow-entitlement")]
public record AwsMediaconnectUpdateFlowEntitlementOptions(
[property: CliOption("--entitlement-arn")] string EntitlementArn,
[property: CliOption("--flow-arn")] string FlowArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--encryption")]
    public string? Encryption { get; set; }

    [CliOption("--entitlement-status")]
    public string? EntitlementStatus { get; set; }

    [CliOption("--subscribers")]
    public string[]? Subscribers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}