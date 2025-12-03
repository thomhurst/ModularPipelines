using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mysql", "flexible-server", "server-logs", "list")]
public record AzMysqlFlexibleServerServerLogsListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName
) : AzOptions
{
    [CliOption("--file-last-written")]
    public string? FileLastWritten { get; set; }

    [CliOption("--filename-contains")]
    public string? FilenameContains { get; set; }

    [CliOption("--max-file-size")]
    public string? MaxFileSize { get; set; }
}