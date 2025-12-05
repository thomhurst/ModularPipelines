using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apic", "api", "definition", "head")]
public record AzApicApiDefinitionHeadOptions : AzOptions
{
    [CliOption("--api")]
    public string? Api { get; set; }

    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}