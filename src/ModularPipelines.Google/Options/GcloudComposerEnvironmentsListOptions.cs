using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments", "list")]
public record GcloudComposerEnvironmentsListOptions : GcloudOptions
{
    [CommandSwitch("--locations")]
    public string[]? Locations { get; set; }
}