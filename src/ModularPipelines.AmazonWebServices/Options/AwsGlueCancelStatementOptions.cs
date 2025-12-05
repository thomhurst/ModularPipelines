using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "cancel-statement")]
public record AwsGlueCancelStatementOptions(
[property: CliOption("--session-id")] string SessionId,
[property: CliOption("--id")] int Id
) : AwsOptions
{
    [CliOption("--request-origin")]
    public string? RequestOrigin { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}