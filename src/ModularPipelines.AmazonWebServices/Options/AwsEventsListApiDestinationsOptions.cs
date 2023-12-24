using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "list-api-destinations")]
public record AwsEventsListApiDestinationsOptions : AwsOptions
{
    [CommandSwitch("--name-prefix")]
    public string? NamePrefix { get; set; }

    [CommandSwitch("--connection-arn")]
    public string? ConnectionArn { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}