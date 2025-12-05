using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "versions", "add")]
public record GcloudSecretsVersionsAddOptions(
[property: CliArgument] string Secret,
[property: CliOption("--data-file")] string DataFile
) : GcloudOptions;