using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "get-compute-auth-token")]
public record AwsGameliftGetComputeAuthTokenOptions(
[property: CommandSwitch("--fleet-id")] string FleetId,
[property: CommandSwitch("--compute-name")] string ComputeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}