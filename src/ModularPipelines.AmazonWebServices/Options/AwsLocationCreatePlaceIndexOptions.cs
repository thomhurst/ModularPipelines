using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("location", "create-place-index")]
public record AwsLocationCreatePlaceIndexOptions(
[property: CommandSwitch("--data-source")] string DataSource,
[property: CommandSwitch("--index-name")] string IndexName
) : AwsOptions
{
    [CommandSwitch("--data-source-configuration")]
    public string? DataSourceConfiguration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--pricing-plan")]
    public string? PricingPlan { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}