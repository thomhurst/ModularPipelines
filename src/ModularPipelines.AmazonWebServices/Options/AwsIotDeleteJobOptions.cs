using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "delete-job")]
public record AwsIotDeleteJobOptions(
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}