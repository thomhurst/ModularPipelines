using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apic", "api", "update")]
public record AzApicApiUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--api")]
    public string? Api { get; set; }

    [CliOption("--contacts")]
    public string? Contacts { get; set; }

    [CliOption("--custom-properties")]
    public string? CustomProperties { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--external-documentation")]
    public string? ExternalDocumentation { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--license")]
    public string? License { get; set; }

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

    [CliOption("--summary")]
    public string? Summary { get; set; }

    [CliOption("--terms-of-service")]
    public string? TermsOfService { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}