using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apic", "api", "deployment", "update")]
public record AzApicApiDeploymentUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--api")]
    public string? Api { get; set; }

    [CliOption("--custom-properties")]
    public string? CustomProperties { get; set; }

    [CliOption("--definition-id")]
    public string? DefinitionId { get; set; }

    [CliOption("--deployment")]
    public string? Deployment { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--workspace")]
    public string? Workspace { get; set; }
}