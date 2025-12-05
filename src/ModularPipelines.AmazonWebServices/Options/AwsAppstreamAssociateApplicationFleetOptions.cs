using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "associate-application-fleet")]
public record AwsAppstreamAssociateApplicationFleetOptions(
[property: CliOption("--fleet-name")] string FleetName,
[property: CliOption("--application-arn")] string ApplicationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}