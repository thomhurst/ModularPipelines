using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "create-agent")]
public record AwsDatasyncCreateAgentOptions(
[property: CommandSwitch("--activation-key")] string ActivationKey
) : AwsOptions
{
    [CommandSwitch("--agent-name")]
    public string? AgentName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--vpc-endpoint-id")]
    public string? VpcEndpointId { get; set; }

    [CommandSwitch("--subnet-arns")]
    public string[]? SubnetArns { get; set; }

    [CommandSwitch("--security-group-arns")]
    public string[]? SecurityGroupArns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}