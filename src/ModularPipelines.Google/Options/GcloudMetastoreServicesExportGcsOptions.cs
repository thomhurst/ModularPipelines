using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "export", "gcs")]
public record GcloudMetastoreServicesExportGcsOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--destination-folder")] string DestinationFolder
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--dump-type")]
    public string? DumpType { get; set; }
}