using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "versions", "add")]
public record GcloudSecretsVersionsAddOptions(
[property: PositionalArgument] string Secret,
[property: CommandSwitch("--data-file")] string DataFile
) : GcloudOptions;