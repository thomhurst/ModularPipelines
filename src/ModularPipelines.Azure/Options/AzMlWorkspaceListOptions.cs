using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "workspace", "list")]
public record AzMlWorkspaceListOptions : AzOptions
{
    [CliOption("--max-results")]
    public string? MaxResults { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}