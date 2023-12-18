using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "versionset", "create")]
public record AzApimApiVersionsetCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--versioning-scheme")] string VersioningScheme
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--version-header-name")]
    public string? VersionHeaderName { get; set; }

    [CommandSwitch("--version-query-name")]
    public string? VersionQueryName { get; set; }

    [CommandSwitch("--version-set-id")]
    public string? VersionSetId { get; set; }
}