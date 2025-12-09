using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "update-app-block-builder")]
public record AwsAppstreamUpdateAppBlockBuilderOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CliOption("--access-endpoints")]
    public string[]? AccessEndpoints { get; set; }

    [CliOption("--attributes-to-delete")]
    public string[]? AttributesToDelete { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}