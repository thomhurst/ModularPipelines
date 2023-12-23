using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "describe-compute")]
public record AwsGameliftDescribeComputeOptions(
[property: CommandSwitch("--fleet-id")] string FleetId,
[property: CommandSwitch("--compute-name")] string ComputeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}