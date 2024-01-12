using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "versions", "list")]
public record GcloudSecretsVersionsListOptions(
[property: PositionalArgument] string Secret
) : GcloudOptions;