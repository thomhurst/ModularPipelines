using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "sbom", "export")]
public record GcloudArtifactsSbomExportOptions(
[property: CommandSwitch("--uri")] string Uri
) : GcloudOptions;