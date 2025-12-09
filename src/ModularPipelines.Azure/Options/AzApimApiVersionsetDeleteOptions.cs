using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "api", "versionset", "delete")]
public record AzApimApiVersionsetDeleteOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--version-set-id")] string VersionSetId
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }
}