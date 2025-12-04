using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcdata", "dc", "upload")]
public record AzArcdataDcUploadOptions(
[property: CliOption("--path")] string Path
) : AzOptions;