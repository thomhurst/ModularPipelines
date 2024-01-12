using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "operations", "delete")]
public record GcloudDataprocOperationsDeleteOptions(
[property: PositionalArgument] string Operation,
[property: PositionalArgument] string Region
) : GcloudOptions;