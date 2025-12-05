using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "operations", "cancel")]
public record GcloudMetastoreOperationsCancelOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;