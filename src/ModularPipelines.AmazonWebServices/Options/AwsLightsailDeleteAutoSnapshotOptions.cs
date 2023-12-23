using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "delete-auto-snapshot")]
public record AwsLightsailDeleteAutoSnapshotOptions(
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--date")] string Date
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}