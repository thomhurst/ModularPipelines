using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "versions", "list")]
public record GcloudSecretsVersionsListOptions(
[property: CliArgument] string Secret
) : GcloudOptions;