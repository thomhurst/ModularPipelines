using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "batch-get-application-revisions")]
public record AwsDeployBatchGetApplicationRevisionsOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--revisions")] string[] Revisions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}