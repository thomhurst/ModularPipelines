using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "blob", "restore")]
public record AzStorageBlobRestoreOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--time-to-restore")] string TimeToRestore
) : AzOptions
{
    [CliOption("--blob-range")]
    public string? BlobRange { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}