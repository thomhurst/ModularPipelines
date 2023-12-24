using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("schemas", "list-discoverers")]
public record AwsSchemasListDiscoverersOptions : AwsOptions
{
    [CommandSwitch("--discoverer-id-prefix")]
    public string? DiscovererIdPrefix { get; set; }

    [CommandSwitch("--source-arn-prefix")]
    public string? SourceArnPrefix { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}