using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "describe-query")]
public record AwsCloudtrailDescribeQueryOptions : AwsOptions
{
    [CommandSwitch("--event-data-store")]
    public string? EventDataStore { get; set; }

    [CommandSwitch("--query-id")]
    public string? QueryId { get; set; }

    [CommandSwitch("--query-alias")]
    public string? QueryAlias { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}