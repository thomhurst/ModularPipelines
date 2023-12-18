using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "versionset", "show")]
public record AzApimApiVersionsetShowOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--version-set-id")] string VersionSetId
) : AzOptions;