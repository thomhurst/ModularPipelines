using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource", "link", "create")]
public record AzResourceLinkCreateOptions(
[property: CommandSwitch("--link")] string Link,
[property: CommandSwitch("--target")] string Target
) : AzOptions
{
    [CommandSwitch("--notes")]
    public string? Notes { get; set; }
}