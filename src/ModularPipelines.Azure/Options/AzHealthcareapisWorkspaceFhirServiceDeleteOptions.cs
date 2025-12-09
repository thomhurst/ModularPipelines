using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("healthcareapis", "workspace", "fhir-service", "delete")]
public record AzHealthcareapisWorkspaceFhirServiceDeleteOptions : AzOptions
{
    [CliOption("--fhir-service-name")]
    public string? FhirServiceName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}