using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cache", "services", "invalidate-cache")]
public record GcloudEdgeCacheServicesInvalidateCacheOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliOption("--host")] string Host,
[property: CliOption("--path")] string Path,
[property: CliOption("--tags")] string[] Tags
) : GcloudOptions;