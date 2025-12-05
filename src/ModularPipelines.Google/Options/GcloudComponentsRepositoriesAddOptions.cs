using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("components", "repositories", "add")]
public record GcloudComponentsRepositoriesAddOptions(
[property: CliArgument] string Url
) : GcloudOptions;