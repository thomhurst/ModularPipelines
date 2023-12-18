using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "list")]
public record AzSpringAppListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--deployment")]
    public string? Deployment { get; set; }

    [BooleanCommandSwitch("--follow")]
    public bool? Follow { get; set; }

    [CommandSwitch("--format-json")]
    public string? FormatJson { get; set; }

    [CommandSwitch("--instance")]
    public string? Instance { get; set; }

    [CommandSwitch("--limit")]
    public string? Limit { get; set; }

    [CommandSwitch("--lines")]
    public string? Lines { get; set; }

    [CommandSwitch("--since")]
    public string? Since { get; set; }
}