using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile", "export-bundle")]
public record AwsMobileExportBundleOptions(
[property: CliOption("--bundle-id")] string BundleId
) : AwsOptions
{
    [CliOption("--project-id")]
    public string? ProjectId { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}