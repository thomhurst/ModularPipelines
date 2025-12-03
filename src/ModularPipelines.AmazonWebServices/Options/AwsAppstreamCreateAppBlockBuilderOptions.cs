using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-app-block-builder")]
public record AwsAppstreamCreateAppBlockBuilderOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--platform")] string Platform,
[property: CliOption("--instance-type")] string InstanceType,
[property: CliOption("--vpc-config")] string VpcConfig
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CliOption("--access-endpoints")]
    public string[]? AccessEndpoints { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}