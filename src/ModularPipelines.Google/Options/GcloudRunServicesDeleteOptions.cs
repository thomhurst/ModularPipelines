using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "services", "delete")]
public record GcloudRunServicesDeleteOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Namespace
) : GcloudOptions
{
    [CliOption("--[no-]async")]
    public string[]? NoAsync { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

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