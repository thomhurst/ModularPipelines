using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("help-search")]
public record NpmHelpSearchOptions : NpmOptions
{
    [BooleanCommandSwitch("--long")]
    public bool? Long { get; set; }
}