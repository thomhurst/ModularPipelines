using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "triggers", "import")]
public record GcloudBuildsTriggersImportOptions(
[property: CommandSwitch("--source")] string Source
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}