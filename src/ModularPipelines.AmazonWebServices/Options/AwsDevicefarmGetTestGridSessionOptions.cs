using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "get-test-grid-session")]
public record AwsDevicefarmGetTestGridSessionOptions : AwsOptions
{
    [CommandSwitch("--project-arn")]
    public string? ProjectArn { get; set; }

    [CommandSwitch("--session-id")]
    public string? SessionId { get; set; }

    [CommandSwitch("--session-arn")]
    public string? SessionArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}