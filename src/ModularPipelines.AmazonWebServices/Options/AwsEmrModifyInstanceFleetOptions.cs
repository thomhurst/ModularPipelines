using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "modify-instance-fleet")]
public record AwsEmrModifyInstanceFleetOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--instance-fleet")] string InstanceFleet
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}