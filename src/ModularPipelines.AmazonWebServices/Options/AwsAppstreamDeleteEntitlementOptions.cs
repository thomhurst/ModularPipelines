using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "delete-entitlement")]
public record AwsAppstreamDeleteEntitlementOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--stack-name")] string StackName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}