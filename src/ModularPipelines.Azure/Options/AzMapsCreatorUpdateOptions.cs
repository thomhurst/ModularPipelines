using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maps", "creator", "update")]
public record AzMapsCreatorUpdateOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--creator-name")]
    public string? CreatorName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--storage-units")]
    public string? StorageUnits { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}