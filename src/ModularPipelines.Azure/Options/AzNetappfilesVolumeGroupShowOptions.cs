using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "volume-group", "show")]
public record AzNetappfilesVolumeGroupShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--group-name")] string GroupName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;