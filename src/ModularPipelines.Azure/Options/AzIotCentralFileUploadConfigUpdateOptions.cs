using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "file-upload-config", "update")]
public record AzIotCentralFileUploadConfigUpdateOptions(
[property: CommandSwitch("--app-id")] string AppId
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [CommandSwitch("--sas-ttl")]
    public string? SasTtl { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}