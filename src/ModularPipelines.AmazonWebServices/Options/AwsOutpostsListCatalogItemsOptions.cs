using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("outposts", "list-catalog-items")]
public record AwsOutpostsListCatalogItemsOptions : AwsOptions
{
    [CliOption("--item-class-filter")]
    public string[]? ItemClassFilter { get; set; }

    [CliOption("--supported-storage-filter")]
    public string[]? SupportedStorageFilter { get; set; }

    [CliOption("--ec2-family-filter")]
    public string[]? Ec2FamilyFilter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}