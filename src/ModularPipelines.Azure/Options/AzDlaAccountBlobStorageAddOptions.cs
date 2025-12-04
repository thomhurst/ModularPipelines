using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dla", "account", "blob-storage", "add")]
public record AzDlaAccountBlobStorageAddOptions(
[property: CliOption("--access-key")] string AccessKey,
[property: CliOption("--storage-account-name")] int StorageAccountName
) : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--suffix")]
    public string? Suffix { get; set; }
}