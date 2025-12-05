using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog", "create-tag-option")]
public record AwsServicecatalogCreateTagOptionOptions(
[property: CliOption("--key")] string Key,
[property: CliOption("--value")] string Value
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}