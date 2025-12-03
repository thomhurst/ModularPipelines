using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("repos", "import", "create")]
public record AzReposImportCreateOptions(
[property: CliOption("--git-source-url")] string GitSourceUrl
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--git-service-endpoint-id")]
    public string? GitServiceEndpointId { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliFlag("--requires-authorization")]
    public bool? RequiresAuthorization { get; set; }

    [CliOption("--user-name")]
    public string? UserName { get; set; }
}