using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutvision", "start-model-packaging-job")]
public record AwsLookoutvisionStartModelPackagingJobOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--model-version")] string ModelVersion,
[property: CliOption("--configuration")] string Configuration
) : AwsOptions
{
    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}