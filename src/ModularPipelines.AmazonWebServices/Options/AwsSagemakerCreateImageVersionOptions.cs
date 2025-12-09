using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-image-version")]
public record AwsSagemakerCreateImageVersionOptions(
[property: CliOption("--base-image")] string BaseImage,
[property: CliOption("--image-name")] string ImageName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--aliases")]
    public string[]? Aliases { get; set; }

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

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}