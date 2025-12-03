using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "associate-fleet")]
public record AwsAppstreamAssociateFleetOptions(
[property: CliOption("--fleet-name")] string FleetName,
[property: CliOption("--stack-name")] string StackName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}