using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "discover-poll-endpoint")]
public record AwsEcsDiscoverPollEndpointOptions : AwsOptions
{
    [CommandSwitch("--container-instance")]
    public string? ContainerInstance { get; set; }

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}