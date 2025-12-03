using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apic", "api", "create")]
public record AzApicApiCreateOptions(
[property: CliOption("--api")] string Api,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--contacts")]
    public string? Contacts { get; set; }

    [CliOption("--custom-properties")]
    public string? CustomProperties { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--external-documentation")]
    public string? ExternalDocumentation { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--license")]
    public string? License { get; set; }

    [CliOption("--summary")]
    public string? Summary { get; set; }

    [CliOption("--terms-of-service")]
    public string? TermsOfService { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}