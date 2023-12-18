using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "createconfig", "hci")]
public record AzArcapplianceCreateconfigHciOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--authenticationport")]
    public string? Authenticationport { get; set; }

    [CommandSwitch("--certificateFilePath")]
    public string? CertificateFilePath { get; set; }

    [CommandSwitch("--cloudagent")]
    public string? Cloudagent { get; set; }

    [CommandSwitch("--controlPlaneEndpoint")]
    public string? ControlPlaneEndpoint { get; set; }

    [CommandSwitch("--dnsservers")]
    public string? Dnsservers { get; set; }

    [CommandSwitch("--gateway")]
    public string? Gateway { get; set; }

    [CommandSwitch("--http")]
    public string? Http { get; set; }

    [CommandSwitch("--https")]
    public string? Https { get; set; }

    [CommandSwitch("--ipaddressprefix")]
    public string? Ipaddressprefix { get; set; }

    [CommandSwitch("--k8snodeippoolend")]
    public string? K8snodeippoolend { get; set; }

    [CommandSwitch("--k8snodeippoolstart")]
    public string? K8snodeippoolstart { get; set; }

    [CommandSwitch("--loginconfigfile")]
    public string? Loginconfigfile { get; set; }

    [CommandSwitch("--noproxy")]
    public string? Noproxy { get; set; }

    [CommandSwitch("--out-dir")]
    public string? OutDir { get; set; }

    [BooleanCommandSwitch("--overwrite-existing")]
    public bool? OverwriteExisting { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--storagecontainer")]
    public string? Storagecontainer { get; set; }

    [CommandSwitch("--vlanid")]
    public string? Vlanid { get; set; }

    [CommandSwitch("--vswitchname")]
    public string? Vswitchname { get; set; }

    [CommandSwitch("--working-dir")]
    public string? WorkingDir { get; set; }
}