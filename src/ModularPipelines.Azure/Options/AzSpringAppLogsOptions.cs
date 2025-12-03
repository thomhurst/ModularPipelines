using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "app", "logs")]
public record AzSpringAppLogsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--deployment")]
    public string? Deployment { get; set; }

    [CliFlag("--follow")]
    public bool? Follow { get; set; }

    [CliOption("--format-json")]
    public string? FormatJson { get; set; }

    [CliOption("--instance")]
    public string? Instance { get; set; }

    [CliOption("--limit")]
    public string? Limit { get; set; }

    [CliOption("--lines")]
    public string? Lines { get; set; }

    [CliOption("--since")]
    public string? Since { get; set; }
}