using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "disassociate-fleet")]
public record AwsAppstreamDisassociateFleetOptions(
[property: CliOption("--fleet-name")] string FleetName,
[property: CliOption("--stack-name")] string StackName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}