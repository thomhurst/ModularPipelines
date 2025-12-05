using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "resiliency", "create")]
public record AzContainerappResiliencyCreateOptions(
[property: CliOption("--container-app-name")] string ContainerAppName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--cb-interval")]
    public string? CbInterval { get; set; }

    [CliOption("--cb-max-ejection")]
    public string? CbMaxEjection { get; set; }

    [CliOption("--cb-sequential-errors")]
    public string? CbSequentialErrors { get; set; }

    [CliOption("--http-codes")]
    public string? HttpCodes { get; set; }

    [CliOption("--http-delay")]
    public string? HttpDelay { get; set; }

    [CliOption("--http-errors")]
    public string? HttpErrors { get; set; }

    [CliOption("--http-interval")]
    public string? HttpInterval { get; set; }

    [CliOption("--http-retries")]
    public string? HttpRetries { get; set; }

    [CliOption("--http1-pending")]
    public string? Http1Pending { get; set; }

    [CliOption("--http2-parallel")]
    public string? Http2Parallel { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--recommended")]
    public bool? Recommended { get; set; }

    [CliOption("--tcp-connections")]
    public string? TcpConnections { get; set; }

    [CliOption("--tcp-retries")]
    public string? TcpRetries { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--timeout-connect")]
    public string? TimeoutConnect { get; set; }

    [CliOption("--yaml")]
    public string? Yaml { get; set; }
}