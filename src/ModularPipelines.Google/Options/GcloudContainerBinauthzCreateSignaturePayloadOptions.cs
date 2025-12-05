using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "create-signature-payload")]
public record GcloudContainerBinauthzCreateSignaturePayloadOptions(
[property: CliOption("--artifact-url")] string ArtifactUrl
) : GcloudOptions;