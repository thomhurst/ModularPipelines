using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-image-version")]
public record AwsSagemakerCreateImageVersionOptions(
[property: CommandSwitch("--base-image")] string BaseImage,
[property: CommandSwitch("--image-name")] string ImageName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--aliases")]
    public string[]? Aliases { get; set; }

    [CommandSwitch("--vendor-guidance")]
    public string? VendorGuidance { get; set; }

    [CommandSwitch("--job-type")]
    public string? JobType { get; set; }

    [CommandSwitch("--ml-framework")]
    public string? MlFramework { get; set; }

    [CommandSwitch("--programming-lang")]
    public string? ProgrammingLang { get; set; }

    [CommandSwitch("--processor")]
    public string? Processor { get; set; }

    [CommandSwitch("--release-notes")]
    public string? ReleaseNotes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}