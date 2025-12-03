using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apic", "api", "definition", "import-specification")]
public record AzApicApiDefinitionImportSpecificationOptions : AzOptions
{
    [CliOption("--api")]
    public string? Api { get; set; }

    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--file-name")]
    public string? FileName { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--specification")]
    public string? Specification { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--value")]
    public string? Value { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}