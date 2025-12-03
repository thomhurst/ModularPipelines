using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "get-resource-share-associations")]
public record AwsRamGetResourceShareAssociationsOptions(
[property: CliOption("--association-type")] string AssociationType
) : AwsOptions
{
    [CliOption("--resource-share-arns")]
    public string[]? ResourceShareArns { get; set; }

    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--principal")]
    public string? Principal { get; set; }

    [CliOption("--association-status")]
    public string? AssociationStatus { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}