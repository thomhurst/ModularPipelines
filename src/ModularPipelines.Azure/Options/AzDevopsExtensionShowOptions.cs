using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "extension", "show")]
public record AzDevopsExtensionShowOptions(
[property: CommandSwitch("--extension-id")] string ExtensionId,
[property: CommandSwitch("--publisher-id")] string PublisherId
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}