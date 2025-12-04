using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "api", "versionset", "create")]
public record AzApimApiVersionsetCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--versioning-scheme")] string VersioningScheme
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--version-header-name")]
    public string? VersionHeaderName { get; set; }

    [CliOption("--version-query-name")]
    public string? VersionQueryName { get; set; }

    [CliOption("--version-set-id")]
    public string? VersionSetId { get; set; }
}