using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "register-targets")]
public record AwsVpcLatticeRegisterTargetsOptions(
[property: CliOption("--target-group-identifier")] string TargetGroupIdentifier,
[property: CliOption("--targets")] string[] Targets
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}