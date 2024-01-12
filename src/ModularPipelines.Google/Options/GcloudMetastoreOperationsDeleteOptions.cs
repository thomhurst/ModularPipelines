using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "operations", "delete")]
public record GcloudMetastoreOperationsDeleteOptions(
[property: PositionalArgument] string Operations,
[property: PositionalArgument] string Location
) : GcloudOptions;