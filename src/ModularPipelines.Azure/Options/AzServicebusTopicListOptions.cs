using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "topic", "list")]
public record AzServicebusTopicListOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--skip")]
    public string? Skip { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}