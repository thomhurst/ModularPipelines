using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguru-reviewer", "list-code-reviews")]
public record AwsCodeguruReviewerListCodeReviewsOptions(
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--provider-types")]
    public string[]? ProviderTypes { get; set; }

    [CommandSwitch("--states")]
    public string[]? States { get; set; }

    [CommandSwitch("--repository-names")]
    public string[]? RepositoryNames { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}