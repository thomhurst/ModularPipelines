using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "associate-targets-with-job")]
public record AwsIotAssociateTargetsWithJobOptions(
[property: CliOption("--targets")] string[] Targets,
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}