using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "add-tags")]
public record AwsEsAddTagsOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--tag-list")] string[] TagList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}