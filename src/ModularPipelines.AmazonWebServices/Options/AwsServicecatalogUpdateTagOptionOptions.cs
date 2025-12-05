using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "update-tag-option")]
public record AwsServicecatalogUpdateTagOptionOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--value")]
    public string? Value { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}