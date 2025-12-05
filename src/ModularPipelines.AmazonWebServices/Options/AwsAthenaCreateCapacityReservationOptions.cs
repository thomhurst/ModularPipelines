using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "create-capacity-reservation")]
public record AwsAthenaCreateCapacityReservationOptions(
[property: CliOption("--target-dpus")] int TargetDpus,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}