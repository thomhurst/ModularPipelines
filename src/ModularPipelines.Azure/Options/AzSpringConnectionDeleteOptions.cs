using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "connection", "delete")]
public record AzSpringConnectionDeleteOptions : AzOptions
{
    [CommandSwitch("--app")]
    public string? App { get; set; }

    [CommandSwitch("--connection")]
    public string? Connection { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}