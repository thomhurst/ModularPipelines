using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedk8s", "proxy")]
public record AzConnectedk8sProxyOptions : AzOptions
{
    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kube-context")]
    public string? KubeContext { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}