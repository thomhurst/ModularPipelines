using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "cloud-bindings", "create")]
public record GcloudAccessContextManagerCloudBindingsCreateOptions(
[property: CommandSwitch("--group-key")] string GroupKey
) : GcloudOptions
{
    [CommandSwitch("--dry-run-level")]
    public string[]? DryRunLevel { get; set; }

    [CommandSwitch("--level")]
    public string[]? Level { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}