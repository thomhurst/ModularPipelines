using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("run", "services", "proxy")]
public record GcloudRunServicesProxyOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Namespace
) : GcloudOptions
{
    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }

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