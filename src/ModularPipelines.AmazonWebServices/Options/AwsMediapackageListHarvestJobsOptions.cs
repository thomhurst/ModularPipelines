using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediapackage", "list-harvest-jobs")]
public record AwsMediapackageListHarvestJobsOptions : AwsOptions
{
    [CommandSwitch("--include-channel-id")]
    public string? IncludeChannelId { get; set; }

    [CommandSwitch("--include-status")]
    public string? IncludeStatus { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}