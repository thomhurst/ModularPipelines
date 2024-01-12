using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "cloud-bindings", "update")]
public record GcloudAccessContextManagerCloudBindingsUpdateOptions(
[property: CommandSwitch("--binding")] string Binding,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions
{
    [CommandSwitch("--dry-run-level")]
    public string[]? DryRunLevel { get; set; }

    [CommandSwitch("--level")]
    public string[]? Level { get; set; }
}