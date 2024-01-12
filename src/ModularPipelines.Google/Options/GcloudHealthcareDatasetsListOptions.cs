using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "datasets", "list")]
public record GcloudHealthcareDatasetsListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}