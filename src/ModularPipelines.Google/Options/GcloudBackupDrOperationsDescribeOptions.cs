using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-dr", "operations", "describe")]
public record GcloudBackupDrOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;