using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "clusters", "get-credentials")]
public record GcloudContainerClustersGetCredentialsOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [BooleanCommandSwitch("--internal-ip")]
    public bool? InternalIp { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}