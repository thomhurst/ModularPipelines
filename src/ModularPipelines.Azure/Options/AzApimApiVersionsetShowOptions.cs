using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "api", "versionset", "show")]
public record AzApimApiVersionsetShowOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--version-set-id")] string VersionSetId
) : AzOptions;