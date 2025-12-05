using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "associate-entities-to-experience")]
public record AwsKendraAssociateEntitiesToExperienceOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--entity-list")] string[] EntityList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}