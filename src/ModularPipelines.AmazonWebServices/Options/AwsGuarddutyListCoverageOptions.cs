using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "list-coverage")]
public record AwsGuarddutyListCoverageOptions(
[property: CommandSwitch("--detector-id")] string DetectorId
) : AwsOptions
{
    [CommandSwitch("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CommandSwitch("--sort-criteria")]
    public string? SortCriteria { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}