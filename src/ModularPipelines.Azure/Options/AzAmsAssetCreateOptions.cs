using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "asset", "create")]
public record AzAmsAssetCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--alternate-id")]
    public string? AlternateId { get; set; }

    [CliOption("--container")]
    public string? Container { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }
}