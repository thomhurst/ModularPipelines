using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "get-savings-plans-utilization")]
public record AwsCeGetSavingsPlansUtilizationOptions(
[property: CommandSwitch("--time-period")] string TimePeriod
) : AwsOptions
{
    [CommandSwitch("--granularity")]
    public string? Granularity { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}