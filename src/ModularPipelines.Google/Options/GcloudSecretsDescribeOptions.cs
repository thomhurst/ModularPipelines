using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("secrets", "describe")]
public record GcloudSecretsDescribeOptions(
[property: CliArgument] string Secret
) : GcloudOptions;