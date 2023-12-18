using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "resiliency", "update")]
public record AzContainerappResiliencyUpdateOptions(
[property: CommandSwitch("--container-app-name")] string ContainerAppName,
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

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

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

