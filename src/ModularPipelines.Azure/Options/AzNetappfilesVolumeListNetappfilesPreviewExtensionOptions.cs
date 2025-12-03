using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "volume", "list", "(netappfiles-preview", "extension)")]
public record AzNetappfilesVolumeListNetappfilesPreviewExtensionOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--pool-name")] string PoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}