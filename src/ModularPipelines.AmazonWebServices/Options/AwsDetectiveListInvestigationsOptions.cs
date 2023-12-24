using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("detective", "list-investigations")]
public record AwsDetectiveListInvestigationsOptions(
[property: CommandSwitch("--graph-arn")] string GraphArn
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CommandSwitch("--sort-criteria")]
    public string? SortCriteria { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}