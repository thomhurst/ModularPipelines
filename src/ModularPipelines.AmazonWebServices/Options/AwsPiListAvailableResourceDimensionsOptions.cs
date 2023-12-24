using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pi", "list-available-resource-dimensions")]
public record AwsPiListAvailableResourceDimensionsOptions(
[property: CommandSwitch("--service-type")] string ServiceType,
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--metrics")] string[] Metrics
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}