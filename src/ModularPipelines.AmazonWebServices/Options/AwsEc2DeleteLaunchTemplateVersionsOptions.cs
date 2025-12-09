using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-launch-template-versions")]
public record AwsEc2DeleteLaunchTemplateVersionsOptions(
[property: CliOption("--versions")] string[] Versions
) : AwsOptions
{
    [CliOption("--launch-template-id")]
    public string? LaunchTemplateId { get; set; }

    [CliOption("--launch-template-name")]
    public string? LaunchTemplateName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}