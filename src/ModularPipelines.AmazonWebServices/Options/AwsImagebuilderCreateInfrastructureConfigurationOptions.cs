using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "create-infrastructure-configuration")]
public record AwsImagebuilderCreateInfrastructureConfigurationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--instance-profile-name")] string InstanceProfileName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--instance-types")]
    public string[]? InstanceTypes { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--logging")]
    public string? Logging { get; set; }

    [CommandSwitch("--key-pair")]
    public string? KeyPair { get; set; }

    [CommandSwitch("--sns-topic-arn")]
    public string? SnsTopicArn { get; set; }

    [CommandSwitch("--resource-tags")]
    public IEnumerable<KeyValue>? ResourceTags { get; set; }

    [CommandSwitch("--instance-metadata-options")]
    public string? InstanceMetadataOptions { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}