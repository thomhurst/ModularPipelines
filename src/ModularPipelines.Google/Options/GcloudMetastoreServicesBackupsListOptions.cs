using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "backups", "list")]
public record GcloudMetastoreServicesBackupsListOptions(
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;