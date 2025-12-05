using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "update-application")]
public record AwsAppstreamUpdateApplicationOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--icon-s3-location")]
    public string? IconS3Location { get; set; }

    [CliOption("--launch-path")]
    public string? LaunchPath { get; set; }

    [CliOption("--working-directory")]
    public string? AwsAppsWorkingDirectory { get; set; }

    [CliOption("--launch-parameters")]
    public string? LaunchParameters { get; set; }

    [CliOption("--app-block-arn")]
    public string? AppBlockArn { get; set; }

    [CliOption("--attributes-to-delete")]
    public string[]? AttributesToDelete { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}