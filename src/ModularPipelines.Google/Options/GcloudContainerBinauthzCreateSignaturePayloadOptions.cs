using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "create-signature-payload")]
public record GcloudContainerBinauthzCreateSignaturePayloadOptions(
[property: CommandSwitch("--artifact-url")] string ArtifactUrl
) : GcloudOptions;