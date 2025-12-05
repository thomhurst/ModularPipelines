using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "operations", "delete")]
public record GcloudMetastoreOperationsDeleteOptions(
[property: CliArgument] string Operations,
[property: CliArgument] string Location
) : GcloudOptions;