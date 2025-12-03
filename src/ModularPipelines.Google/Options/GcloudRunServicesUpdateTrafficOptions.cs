using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "services", "update-traffic")]
public record GcloudRunServicesUpdateTrafficOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Namespace
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliFlag("--clear-tags")]
    public bool? ClearTags { get; set; }

    [CliOption("--set-tags")]
    public string[]? SetTags { get; set; }

    [CliOption("--remove-tags")]
    public string[]? RemoveTags { get; set; }

    [CliOption("--update-tags")]
    public string[]? UpdateTags { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [CliFlag("--to-latest")]
    public bool? ToLatest { get; set; }

    [CliOption("--to-revisions")]
    public string[]? ToRevisions { get; set; }

    [CliOption("--to-tags")]
    public string[]? ToTags { get; set; }
}