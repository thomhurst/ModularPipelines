using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "describe-featured-results-set")]
public record AwsKendraDescribeFeaturedResultsSetOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--featured-results-set-id")] string FeaturedResultsSetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}