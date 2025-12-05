using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "run-statement")]
public record AwsGlueRunStatementOptions(
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--code")] string Code
) : AwsOptions
{
    [CliOption("--request-origin")]
    public string? RequestOrigin { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}