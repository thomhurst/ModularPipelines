using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apic", "api", "definition", "update")]
public record AzApicApiDefinitionUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--api")]
    public string? Api { get; set; }

    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}