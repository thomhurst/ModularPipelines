using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "update-listener")]
public record AwsGlobalacceleratorUpdateListenerOptions(
[property: CommandSwitch("--listener-arn")] string ListenerArn
) : AwsOptions
{
    [CommandSwitch("--port-ranges")]
    public string[]? PortRanges { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--client-affinity")]
    public string? ClientAffinity { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}