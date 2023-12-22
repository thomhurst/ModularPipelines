using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "server-logs", "list")]
public record AzMysqlServerLogsListOptions(
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