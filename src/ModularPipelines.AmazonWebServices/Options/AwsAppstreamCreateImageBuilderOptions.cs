using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-image-builder")]
public record AwsAppstreamCreateImageBuilderOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--instance-type")] string InstanceType
) : AwsOptions
{
    [CliOption("--image-name")]
    public string? ImageName { get; set; }

    [CliOption("--image-arn")]
    public string? ImageArn { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CliOption("--domain-join-info")]
    public string? DomainJoinInfo { get; set; }

    [CliOption("--appstream-agent-version")]
    public string? AppstreamAgentVersion { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--access-endpoints")]
    public string[]? AccessEndpoints { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}