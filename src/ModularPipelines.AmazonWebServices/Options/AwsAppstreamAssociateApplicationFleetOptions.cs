using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "associate-application-fleet")]
public record AwsAppstreamAssociateApplicationFleetOptions(
[property: CommandSwitch("--fleet-name")] string FleetName,
[property: CommandSwitch("--application-arn")] string ApplicationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}