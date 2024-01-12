using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "triggers", "list")]
public record GcloudEventarcTriggersListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}