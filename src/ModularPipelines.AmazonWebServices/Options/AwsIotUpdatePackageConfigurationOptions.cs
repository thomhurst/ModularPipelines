using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-package-configuration")]
public record AwsIotUpdatePackageConfigurationOptions : AwsOptions
{
    [CliOption("--version-update-by-jobs-config")]
    public string? VersionUpdateByJobsConfig { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}