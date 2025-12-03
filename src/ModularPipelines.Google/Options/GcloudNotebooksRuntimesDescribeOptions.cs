using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "runtimes", "describe")]
public record GcloudNotebooksRuntimesDescribeOptions(
[property: CliArgument] string Runtime,
[property: CliArgument] string Location
) : GcloudOptions;