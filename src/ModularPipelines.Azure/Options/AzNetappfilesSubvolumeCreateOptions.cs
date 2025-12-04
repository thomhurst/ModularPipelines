using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "subvolume", "create")]
public record AzNetappfilesSubvolumeCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subvolume-name")] string SubvolumeName,
[property: CliOption("--volume-name")] string VolumeName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--parent-path")]
    public string? ParentPath { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--size")]
    public string? Size { get; set; }
}