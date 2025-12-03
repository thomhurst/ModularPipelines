using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("panorama", "create-package-import-job")]
public record AwsPanoramaCreatePackageImportJobOptions(
[property: CliOption("--client-token")] string ClientToken,
[property: CliOption("--input-config")] string InputConfig,
[property: CliOption("--job-type")] string JobType,
[property: CliOption("--output-config")] string OutputConfig
) : AwsOptions
{
    [CliOption("--job-tags")]
    public string[]? JobTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}