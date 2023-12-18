using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "subscription", "rename")]
public record AzAccountSubscriptionRenameOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}