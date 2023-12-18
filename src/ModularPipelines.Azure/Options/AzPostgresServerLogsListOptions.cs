using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "server-logs", "list")]
public record AzPostgresServerLogsListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server-name")] string ServerName
) : AzOptions
{
    [CommandSwitch("--file-last-written")]
    public string? FileLastWritten { get; set; }

    [CommandSwitch("--filename-contains")]
    public string? FilenameContains { get; set; }

    [CommandSwitch("--max-file-size")]
    public string? MaxFileSize { get; set; }
}