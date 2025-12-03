using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "list-resources")]
public record AwsRamListResourcesOptions(
[property: CliOption("--resource-owner")] string ResourceOwner
) : AwsOptions
{
    [CliOption("--principal")]
    public string? Principal { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--resource-arns")]
    public string[]? ResourceArns { get; set; }

    [CliOption("--resource-share-arns")]
    public string[]? ResourceShareArns { get; set; }

    [CliOption("--resource-region-scope")]
    public string? ResourceRegionScope { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}