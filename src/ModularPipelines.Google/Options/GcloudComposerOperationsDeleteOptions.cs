using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "operations", "delete")]
public record GcloudComposerOperationsDeleteOptions(
[property: CliArgument] string Operations,
[property: CliArgument] string Location
) : GcloudOptions;