using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("support", "tickets", "list")]
public record AzSupportTicketsListOptions : AzOptions
{
    [CliOption("--filters")]
    public string? Filters { get; set; }
}