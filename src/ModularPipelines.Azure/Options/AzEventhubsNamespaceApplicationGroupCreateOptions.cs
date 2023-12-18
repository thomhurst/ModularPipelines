using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace", "application-group", "create")]
public record AzEventhubsNamespaceApplicationGroupCreateOptions(
[property: CommandSwitch("--client-app-group-id")] string ClientAppGroupId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--is-enabled")]
    public bool? IsEnabled { get; set; }

    [CommandSwitch("--policy-config")]
    public string? PolicyConfig { get; set; }
}