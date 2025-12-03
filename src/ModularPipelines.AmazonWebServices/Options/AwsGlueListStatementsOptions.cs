using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "list-statements")]
public record AwsGlueListStatementsOptions(
[property: CliOption("--session-id")] string SessionId
) : AwsOptions
{
    [CliOption("--request-origin")]
    public string? RequestOrigin { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}