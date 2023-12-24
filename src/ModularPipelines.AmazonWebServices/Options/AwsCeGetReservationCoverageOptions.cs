using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "get-reservation-coverage")]
public record AwsCeGetReservationCoverageOptions(
[property: CommandSwitch("--time-period")] string TimePeriod
) : AwsOptions
{
    [CommandSwitch("--group-by")]
    public string[]? GroupBy { get; set; }

    [CommandSwitch("--granularity")]
    public string? Granularity { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--metrics")]
    public string[]? Metrics { get; set; }

    [CommandSwitch("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}