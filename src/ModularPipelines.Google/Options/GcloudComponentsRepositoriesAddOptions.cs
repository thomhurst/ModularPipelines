using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("components", "repositories", "add")]
public record GcloudComponentsRepositoriesAddOptions(
[property: PositionalArgument] string Url
) : GcloudOptions;