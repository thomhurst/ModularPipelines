using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "discover-poll-endpoint")]
public record AwsEcsDiscoverPollEndpointOptions : AwsOptions
{
    [CliOption("--container-instance")]
    public string? ContainerInstance { get; set; }

    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}