using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "get-support-data")]
public record AzSphereGetSupportDataOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--output-file")] string OutputFile,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;