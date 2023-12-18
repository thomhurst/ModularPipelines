using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "logs")]
public record AzSpringAppLogsOptions(
[property: CommandSwitch("--name")] string Name,
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