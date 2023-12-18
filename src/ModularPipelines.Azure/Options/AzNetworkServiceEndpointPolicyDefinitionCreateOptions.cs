using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "service-endpoint", "policy-definition", "create")]
public record AzNetworkServiceEndpointPolicyDefinitionCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--service-resources")]
    public string? ServiceResources { get; set; }
}