using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "update-target-group")]
public record AwsVpcLatticeUpdateTargetGroupOptions(
[property: CliOption("--health-check")] string HealthCheck,
[property: CliOption("--target-group-identifier")] string TargetGroupIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}