using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "revisions", "list")]
public record GcloudRunRevisionsListOptions : GcloudOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

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
}