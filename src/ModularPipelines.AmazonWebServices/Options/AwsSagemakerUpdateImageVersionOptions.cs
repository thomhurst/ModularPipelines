using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "update-image-version")]
public record AwsSagemakerUpdateImageVersionOptions(
[property: CliOption("--image-name")] string ImageName
) : AwsOptions
{
    [CliOption("--alias")]
    public string? Alias { get; set; }

    [CliOption("--aliases-to-add")]
    public string[]? AliasesToAdd { get; set; }

    [CliOption("--aliases-to-delete")]
    public string[]? AliasesToDelete { get; set; }

    [CliOption("--vendor-guidance")]
    public string? VendorGuidance { get; set; }

    [CliOption("--job-type")]
    public string? JobType { get; set; }

    [CliOption("--ml-framework")]
    public string? MlFramework { get; set; }

    [CliOption("--programming-lang")]
    public string? ProgrammingLang { get; set; }

    [CliOption("--processor")]
    public string? Processor { get; set; }

    [CliOption("--release-notes")]
    public string? ReleaseNotes { get; set; }

    [CliOption("--version-number")]
    public int? VersionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}