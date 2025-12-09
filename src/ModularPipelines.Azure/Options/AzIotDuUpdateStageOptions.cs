using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "update", "stage")]
public record AzIotDuUpdateStageOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--manifest-path")] string ManifestPath,
[property: CliOption("--storage-account")] int StorageAccount,
[property: CliOption("--storage-container")] string StorageContainer
) : AzOptions
{
    [CliOption("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CliFlag("--overwrite")]
    public bool? Overwrite { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--storage-subscription")]
    public string? StorageSubscription { get; set; }

    [CliFlag("--then-import")]
    public bool? ThenImport { get; set; }
}