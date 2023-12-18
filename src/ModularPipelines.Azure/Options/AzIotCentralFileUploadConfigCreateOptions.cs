using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "file-upload-config", "create")]
public record AzIotCentralFileUploadConfigCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--connection-string")] string ConnectionString,
[property: CommandSwitch("--container")] string Container
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--sas-ttl")]
    public string? SasTtl { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}