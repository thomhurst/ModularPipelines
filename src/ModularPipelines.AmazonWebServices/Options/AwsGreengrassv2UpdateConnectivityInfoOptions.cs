using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrassv2", "update-connectivity-info")]
public record AwsGreengrassv2UpdateConnectivityInfoOptions(
[property: CommandSwitch("--thing-name")] string ThingName,
[property: CommandSwitch("--connectivity-info")] string[] ConnectivityInfo
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}