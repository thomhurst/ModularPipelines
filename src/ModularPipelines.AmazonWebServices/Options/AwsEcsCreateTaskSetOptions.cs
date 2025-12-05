using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "create-task-set")]
public record AwsEcsCreateTaskSetOptions(
[property: CliOption("--service")] string Service,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--task-definition")] string TaskDefinition
) : AwsOptions
{
    [CliOption("--external-id")]
    public string? ExternalId { get; set; }

    [CliOption("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CliOption("--load-balancers")]
    public string[]? LoadBalancers { get; set; }

    [CliOption("--service-registries")]
    public string[]? ServiceRegistries { get; set; }

    [CliOption("--launch-type")]
    public string? LaunchType { get; set; }

    [CliOption("--capacity-provider-strategy")]
    public string[]? CapacityProviderStrategy { get; set; }

    [CliOption("--platform-version")]
    public string? PlatformVersion { get; set; }

    [CliOption("--scale")]
    public string? Scale { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}