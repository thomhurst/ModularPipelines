using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "instance-pool", "create")]
public record AzSqlInstancePoolCreateOptions(
[property: CliOption("--capacity")] string Capacity,
[property: CliOption("--edition")] string Edition,
[property: CliOption("--family")] string Family,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subnet")] string Subnet
) : AzOptions
{
    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--maint-config-id")]
    public string? MaintConfigId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}