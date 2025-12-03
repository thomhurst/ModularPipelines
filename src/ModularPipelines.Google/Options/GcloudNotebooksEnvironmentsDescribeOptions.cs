using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "environments", "describe")]
public record GcloudNotebooksEnvironmentsDescribeOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Location
) : GcloudOptions;