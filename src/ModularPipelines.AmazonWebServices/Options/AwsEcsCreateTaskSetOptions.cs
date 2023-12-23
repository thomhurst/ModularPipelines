using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "create-task-set")]
public record AwsEcsCreateTaskSetOptions(
[property: CommandSwitch("--service")] string Service,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--task-definition")] string TaskDefinition
) : AwsOptions
{
    [CommandSwitch("--external-id")]
    public string? ExternalId { get; set; }

    [CommandSwitch("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CommandSwitch("--load-balancers")]
    public string[]? LoadBalancers { get; set; }

    [CommandSwitch("--service-registries")]
    public string[]? ServiceRegistries { get; set; }

    [CommandSwitch("--launch-type")]
    public string? LaunchType { get; set; }

    [CommandSwitch("--capacity-provider-strategy")]
    public string[]? CapacityProviderStrategy { get; set; }

    [CommandSwitch("--platform-version")]
    public string? PlatformVersion { get; set; }

    [CommandSwitch("--scale")]
    public string? Scale { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}