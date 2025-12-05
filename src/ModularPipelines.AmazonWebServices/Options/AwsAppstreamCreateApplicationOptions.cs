using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-application")]
public record AwsAppstreamCreateApplicationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--icon-s3-location")] string IconS3Location,
[property: CliOption("--launch-path")] string LaunchPath,
[property: CliOption("--platforms")] string[] Platforms,
[property: CliOption("--instance-families")] string[] InstanceFamilies,
[property: CliOption("--app-block-arn")] string AppBlockArn
) : AwsOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--working-directory")]
    public string? AwsAppsWorkingDirectory { get; set; }

    [CliOption("--launch-parameters")]
    public string? LaunchParameters { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}