using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "tickets", "list")]
public record AzSupportTicketsListOptions : AzOptions
{
    [CommandSwitch("--filters")]
    public string? Filters { get; set; }
}