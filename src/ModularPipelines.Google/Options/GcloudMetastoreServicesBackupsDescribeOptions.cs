using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "backups", "describe")]
public record GcloudMetastoreServicesBackupsDescribeOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Service
) : GcloudOptions;