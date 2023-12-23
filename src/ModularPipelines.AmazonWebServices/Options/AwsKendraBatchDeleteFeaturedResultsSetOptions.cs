using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "batch-delete-featured-results-set")]
public record AwsKendraBatchDeleteFeaturedResultsSetOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--featured-results-set-ids")] string[] FeaturedResultsSetIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}