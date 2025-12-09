using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "stop-session")]
public record AwsGlueStopSessionOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--request-origin")]
    public string? RequestOrigin { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}