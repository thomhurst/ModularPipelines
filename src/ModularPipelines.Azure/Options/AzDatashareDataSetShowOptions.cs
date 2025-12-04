using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datashare", "data-set", "show")]
public record AzDatashareDataSetShowOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--data-set-name")]
    public string? DataSetName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--share-name")]
    public string? ShareName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}