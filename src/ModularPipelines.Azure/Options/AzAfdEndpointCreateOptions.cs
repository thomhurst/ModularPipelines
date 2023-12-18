using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "endpoint", "create")]
public record AzAfdEndpointCreateOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--enabled-state")]
    public bool? EnabledState { get; set; }

    [CommandSwitch("--name-reuse-scope")]
    public string? NameReuseScope { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}