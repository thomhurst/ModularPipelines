using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edge-cache", "services", "invalidate-cache")]
public record GcloudEdgeCacheServicesInvalidateCacheOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--host")] string Host,
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--tags")] string[] Tags
) : GcloudOptions;