using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("outposts", "list-catalog-items")]
public record AwsOutpostsListCatalogItemsOptions : AwsOptions
{
    [CommandSwitch("--item-class-filter")]
    public string[]? ItemClassFilter { get; set; }

    [CommandSwitch("--supported-storage-filter")]
    public string[]? SupportedStorageFilter { get; set; }

    [CommandSwitch("--ec2-family-filter")]
    public string[]? Ec2FamilyFilter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}