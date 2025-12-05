using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "create-agent")]
public record AwsDatasyncCreateAgentOptions(
[property: CliOption("--activation-key")] string ActivationKey
) : AwsOptions
{
    [CliOption("--agent-name")]
    public string? AgentName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--vpc-endpoint-id")]
    public string? VpcEndpointId { get; set; }

    [CliOption("--subnet-arns")]
    public string[]? SubnetArns { get; set; }

    [CliOption("--security-group-arns")]
    public string[]? SecurityGroupArns { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}