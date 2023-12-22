using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "resiliency", "create")]
public record AzContainerappResiliencyCreateOptions(
[property: CommandSwitch("--container-app-name")] string ContainerAppName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--cb-interval")]
    public string? CbInterval { get; set; }

    [CommandSwitch("--cb-max-ejection")]
    public string? CbMaxEjection { get; set; }

    [CommandSwitch("--cb-sequential-errors")]
    public string? CbSequentialErrors { get; set; }

    [CommandSwitch("--http-codes")]
    public string? HttpCodes { get; set; }

    [CommandSwitch("--http-delay")]
    public string? HttpDelay { get; set; }

    [CommandSwitch("--http-errors")]
    public string? HttpErrors { get; set; }

    [CommandSwitch("--http-interval")]
    public string? HttpInterval { get; set; }

    [CommandSwitch("--http-retries")]
    public string? HttpRetries { get; set; }

    [CommandSwitch("--http1-pending")]
    public string? Http1Pending { get; set; }

    [CommandSwitch("--http2-parallel")]
    public string? Http2Parallel { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--recommended")]
    public bool? Recommended { get; set; }

    [CommandSwitch("--tcp-connections")]
    public string? TcpConnections { get; set; }

    [CommandSwitch("--tcp-retries")]
    public string? TcpRetries { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--timeout-connect")]
    public string? TimeoutConnect { get; set; }

    [CommandSwitch("--yaml")]
    public string? Yaml { get; set; }
}