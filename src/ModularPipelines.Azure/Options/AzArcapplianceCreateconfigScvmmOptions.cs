using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcappliance", "createconfig", "scvmm")]
public record AzArcapplianceCreateconfigScvmmOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--certificateFilePath")]
    public string? CertificateFilePath { get; set; }

    [CliOption("--cloudname")]
    public string? Cloudname { get; set; }

    [CliOption("--controlPlaneEndpoint")]
    public string? ControlPlaneEndpoint { get; set; }

    [CliOption("--dnsservers")]
    public string? Dnsservers { get; set; }

    [CliOption("--gateway")]
    public string? Gateway { get; set; }

    [CliOption("--highlyavailable")]
    public string? Highlyavailable { get; set; }

    [CliOption("--hostgroupid")]
    public string? Hostgroupid { get; set; }

    [CliOption("--http")]
    public string? Http { get; set; }

    [CliOption("--https")]
    public string? Https { get; set; }

    [CliOption("--ipaddressprefix")]
    public string? Ipaddressprefix { get; set; }

    [CliOption("--ippool")]
    public string? Ippool { get; set; }

    [CliOption("--k8snodeippoolend")]
    public string? K8snodeippoolend { get; set; }

    [CliOption("--k8snodeippoolstart")]
    public string? K8snodeippoolstart { get; set; }

    [CliOption("--libshare")]
    public string? Libshare { get; set; }

    [CliOption("--macaddress")]
    public string? Macaddress { get; set; }

    [CliOption("--memorymib")]
    public string? Memorymib { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--noproxy")]
    public string? Noproxy { get; set; }

    [CliOption("--numcpus")]
    public string? Numcpus { get; set; }

    [CliOption("--out-dir")]
    public string? OutDir { get; set; }

    [CliFlag("--overwrite-existing")]
    public bool? OverwriteExisting { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--working-dir")]
    public string? WorkingDir { get; set; }
}