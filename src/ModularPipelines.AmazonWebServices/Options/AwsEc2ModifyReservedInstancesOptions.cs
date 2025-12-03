using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-reserved-instances")]
public record AwsEc2ModifyReservedInstancesOptions(
[property: CliOption("--reserved-instances-ids")] string[] ReservedInstancesIds,
[property: CliOption("--target-configurations")] string[] TargetConfigurations
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}