using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "associate-application-to-entitlement")]
public record AwsAppstreamAssociateApplicationToEntitlementOptions(
[property: CliOption("--stack-name")] string StackName,
[property: CliOption("--entitlement-name")] string EntitlementName,
[property: CliOption("--application-identifier")] string ApplicationIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}