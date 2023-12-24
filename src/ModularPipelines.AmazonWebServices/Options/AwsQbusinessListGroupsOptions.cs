using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "list-groups")]
public record AwsQbusinessListGroupsOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--updated-earlier-than")] long UpdatedEarlierThan
) : AwsOptions
{
    [CommandSwitch("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}