using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "describe")]
public record GcloudComposerEnvironmentsDescribeOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Location
) : GcloudOptions;