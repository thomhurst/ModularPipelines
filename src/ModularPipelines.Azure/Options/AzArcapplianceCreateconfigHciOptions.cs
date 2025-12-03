using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcappliance", "createconfig", "hci")]
public record AzArcapplianceCreateconfigHciOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--authenticationport")]
    public string? Authenticationport { get; set; }

    [CliOption("--certificateFilePath")]
    public string? CertificateFilePath { get; set; }

    [CliOption("--cloudagent")]
    public string? Cloudagent { get; set; }

    [CliOption("--controlPlaneEndpoint")]
    public string? ControlPlaneEndpoint { get; set; }

    [CliOption("--dnsservers")]
    public string? Dnsservers { get; set; }

    [CliOption("--gateway")]
    public string? Gateway { get; set; }

    [CliOption("--http")]
    public string? Http { get; set; }

    [CliOption("--https")]
    public string? Https { get; set; }

    [CliOption("--ipaddressprefix")]
    public string? Ipaddressprefix { get; set; }

    [CliOption("--k8snodeippoolend")]
    public string? K8snodeippoolend { get; set; }

    [CliOption("--k8snodeippoolstart")]
    public string? K8snodeippoolstart { get; set; }

    [CliOption("--loginconfigfile")]
    public string? Loginconfigfile { get; set; }

    [CliOption("--noproxy")]
    public string? Noproxy { get; set; }

    [CliOption("--out-dir")]
    public string? OutDir { get; set; }

    [CliFlag("--overwrite-existing")]
    public bool? OverwriteExisting { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--storagecontainer")]
    public string? Storagecontainer { get; set; }

    [CliOption("--vlanid")]
    public string? Vlanid { get; set; }

    [CliOption("--vswitchname")]
    public string? Vswitchname { get; set; }

    [CliOption("--working-dir")]
    public string? WorkingDir { get; set; }
}