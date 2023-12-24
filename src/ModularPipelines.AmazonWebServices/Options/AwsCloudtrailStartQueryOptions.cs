using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "start-query")]
public record AwsCloudtrailStartQueryOptions : AwsOptions
{
    [CommandSwitch("--query-statement")]
    public string? QueryStatement { get; set; }

    [CommandSwitch("--delivery-s3-uri")]
    public string? DeliveryS3Uri { get; set; }

    [CommandSwitch("--query-alias")]
    public string? QueryAlias { get; set; }

    [CommandSwitch("--query-parameters")]
    public string[]? QueryParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}