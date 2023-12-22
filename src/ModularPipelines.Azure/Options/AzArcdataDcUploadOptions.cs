using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "upload")]
public record AzArcdataDcUploadOptions(
[property: CommandSwitch("--path")] string Path
) : AzOptions;