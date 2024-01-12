using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "operations", "list")]
public record GcloudComposerOperationsListOptions : GcloudOptions
{
    [CommandSwitch("--locations")]
    public string[]? Locations { get; set; }
}