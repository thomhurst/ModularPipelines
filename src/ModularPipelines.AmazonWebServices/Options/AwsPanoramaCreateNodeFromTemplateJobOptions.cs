using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("panorama", "create-node-from-template-job")]
public record AwsPanoramaCreateNodeFromTemplateJobOptions(
[property: CliOption("--node-name")] string NodeName,
[property: CliOption("--output-package-name")] string OutputPackageName,
[property: CliOption("--output-package-version")] string OutputPackageVersion,
[property: CliOption("--template-parameters")] IEnumerable<KeyValue> TemplateParameters,
[property: CliOption("--template-type")] string TemplateType
) : AwsOptions
{
    [CliOption("--job-tags")]
    public string[]? JobTags { get; set; }

    [CliOption("--node-description")]
    public string? NodeDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}