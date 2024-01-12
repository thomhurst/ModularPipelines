using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "backups", "create")]
public record GcloudMetastoreServicesBackupsCreateOptions(
[property: PositionalArgument] string Backup,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Service
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }
}