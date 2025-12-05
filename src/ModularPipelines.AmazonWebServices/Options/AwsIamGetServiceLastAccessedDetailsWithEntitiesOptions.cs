using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "get-service-last-accessed-details-with-entities")]
public record AwsIamGetServiceLastAccessedDetailsWithEntitiesOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--service-namespace")] string ServiceNamespace
) : AwsOptions
{
    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}