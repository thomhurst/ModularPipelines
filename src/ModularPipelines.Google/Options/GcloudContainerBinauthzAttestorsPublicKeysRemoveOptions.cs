using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "attestors", "public-keys", "remove")]
public record GcloudContainerBinauthzAttestorsPublicKeysRemoveOptions(
[property: PositionalArgument] string PublicKeyId,
[property: CommandSwitch("--attestor")] string Attestor
) : GcloudOptions;