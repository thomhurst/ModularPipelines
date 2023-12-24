using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "associate-fleet")]
public record AwsAppstreamAssociateFleetOptions(
[property: CommandSwitch("--fleet-name")] string FleetName,
[property: CommandSwitch("--stack-name")] string StackName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}