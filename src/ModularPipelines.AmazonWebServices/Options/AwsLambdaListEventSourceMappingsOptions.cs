using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "list-event-source-mappings")]
public record AwsLambdaListEventSourceMappingsOptions : AwsOptions
{
    [CommandSwitch("--event-source-arn")]
    public string? EventSourceArn { get; set; }

    [CommandSwitch("--function-name")]
    public string? FunctionName { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}