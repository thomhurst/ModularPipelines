using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-image-version")]
public record AwsSagemakerUpdateImageVersionOptions(
[property: CommandSwitch("--image-name")] string ImageName
) : AwsOptions
{
    [CommandSwitch("--alias")]
    public string? Alias { get; set; }

    [CommandSwitch("--aliases-to-add")]
    public string[]? AliasesToAdd { get; set; }

    [CommandSwitch("--aliases-to-delete")]
    public string[]? AliasesToDelete { get; set; }

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

    [CommandSwitch("--version-number")]
    public int? VersionNumber { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}