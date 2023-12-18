using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "release", "show")]
public record AzApimApiReleaseShowOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--release-id")] string ReleaseId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions;