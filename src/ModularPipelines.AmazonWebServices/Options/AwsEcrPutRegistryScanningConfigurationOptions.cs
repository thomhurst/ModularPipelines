using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "put-registry-scanning-configuration")]
public record AwsEcrPutRegistryScanningConfigurationOptions : AwsOptions
{
    [CliOption("--scan-type")]
    public string? ScanType { get; set; }

    [CliOption("--rules")]
    public string[]? Rules { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}