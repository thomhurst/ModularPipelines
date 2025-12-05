using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-launch-template-version")]
public record AwsEc2CreateLaunchTemplateVersionOptions(
[property: CliOption("--launch-template-data")] string LaunchTemplateData
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--launch-template-id")]
    public string? LaunchTemplateId { get; set; }

    [CliOption("--launch-template-name")]
    public string? LaunchTemplateName { get; set; }

    [CliOption("--source-version")]
    public string? SourceVersion { get; set; }

    [CliOption("--version-description")]
    public string? VersionDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}