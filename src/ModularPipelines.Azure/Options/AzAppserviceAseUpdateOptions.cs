using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "ase", "update")]
public record AzAppserviceAseUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--allow-incoming-ftp-connections")]
    public bool? AllowIncomingFtpConnections { get; set; }

    [BooleanCommandSwitch("--allow-new-private-endpoint-connections")]
    public bool? AllowNewPrivateEndpointConnections { get; set; }

    [BooleanCommandSwitch("--allow-remote-debugging")]
    public bool? AllowRemoteDebugging { get; set; }

    [CommandSwitch("--front-end-scale-factor")]
    public string? FrontEndScaleFactor { get; set; }

    [CommandSwitch("--front-end-sku")]
    public string? FrontEndSku { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}