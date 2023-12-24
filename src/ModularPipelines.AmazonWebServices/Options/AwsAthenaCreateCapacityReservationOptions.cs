using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "create-capacity-reservation")]
public record AwsAthenaCreateCapacityReservationOptions(
[property: CommandSwitch("--target-dpus")] int TargetDpus,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}