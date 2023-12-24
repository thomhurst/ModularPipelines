using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "list-groups-older-than-ordering-id")]
public record AwsKendraListGroupsOlderThanOrderingIdOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--ordering-id")] long OrderingId
) : AwsOptions
{
    [CommandSwitch("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}