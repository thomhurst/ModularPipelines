using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "backups", "list")]
public record GcloudMetastoreServicesBackupsListOptions(
[property: CliOption("--service")] string Service,
[property: CliOption("--location")] string Location
) : GcloudOptions;