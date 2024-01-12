using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notebooks", "runtimes", "describe")]
public record GcloudNotebooksRuntimesDescribeOptions(
[property: PositionalArgument] string Runtime,
[property: PositionalArgument] string Location
) : GcloudOptions;