using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "backups", "delete")]
public record GcloudBigtableBackupsDeleteOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Instance
) : GcloudOptions;