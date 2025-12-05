using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "create-infrastructure-configuration")]
public record AwsImagebuilderCreateInfrastructureConfigurationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--instance-profile-name")] string InstanceProfileName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--instance-types")]
    public string[]? InstanceTypes { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--logging")]
    public string? Logging { get; set; }

    [CliOption("--key-pair")]
    public string? KeyPair { get; set; }

    [CliOption("--sns-topic-arn")]
    public string? SnsTopicArn { get; set; }

    [CliOption("--resource-tags")]
    public IEnumerable<KeyValue>? ResourceTags { get; set; }

    [CliOption("--instance-metadata-options")]
    public string? InstanceMetadataOptions { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}