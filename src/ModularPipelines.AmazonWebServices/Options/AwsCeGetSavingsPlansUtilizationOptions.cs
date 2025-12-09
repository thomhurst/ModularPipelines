using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "get-savings-plans-utilization")]
public record AwsCeGetSavingsPlansUtilizationOptions(
[property: CliOption("--time-period")] string TimePeriod
) : AwsOptions
{
    [CliOption("--granularity")]
    public string? Granularity { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}