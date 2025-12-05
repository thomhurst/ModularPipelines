using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "operations", "describe")]
public record GcloudComposerOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;