using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "flexible-server", "parameter", "list")]
public record AzMysqlFlexibleServerParameterListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName
) : AzOptions
{
    [CliOption("--keyword")]
    public string? Keyword { get; set; }

    [CliOption("--page")]
    public string? Page { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}