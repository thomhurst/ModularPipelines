using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-launch-template")]
public record AwsEc2ModifyLaunchTemplateOptions : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--launch-template-id")]
    public string? LaunchTemplateId { get; set; }

    [CliOption("--launch-template-name")]
    public string? LaunchTemplateName { get; set; }

    [CliOption("--default-version")]
    public string? DefaultVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}