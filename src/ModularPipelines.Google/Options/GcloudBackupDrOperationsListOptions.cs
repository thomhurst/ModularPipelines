using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-dr", "operations", "list")]
public record GcloudBackupDrOperationsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;