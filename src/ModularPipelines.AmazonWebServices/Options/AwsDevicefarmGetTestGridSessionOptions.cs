using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "get-test-grid-session")]
public record AwsDevicefarmGetTestGridSessionOptions : AwsOptions
{
    [CliOption("--project-arn")]
    public string? ProjectArn { get; set; }

    [CliOption("--session-id")]
    public string? SessionId { get; set; }

    [CliOption("--session-arn")]
    public string? SessionArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}