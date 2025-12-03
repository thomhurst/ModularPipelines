using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "tickets", "list")]
public record AzSupportTicketsListOptions : AzOptions
{
    [CliOption("--filters")]
    public string? Filters { get; set; }
}