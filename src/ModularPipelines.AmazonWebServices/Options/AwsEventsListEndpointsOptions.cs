using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "list-endpoints")]
public record AwsEventsListEndpointsOptions : AwsOptions
{
    [CommandSwitch("--name-prefix")]
    public string? NamePrefix { get; set; }

    [CommandSwitch("--home-region")]
    public string? HomeRegion { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}