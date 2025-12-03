using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "sbom", "export")]
public record GcloudArtifactsSbomExportOptions(
[property: CliOption("--uri")] string Uri
) : GcloudOptions;