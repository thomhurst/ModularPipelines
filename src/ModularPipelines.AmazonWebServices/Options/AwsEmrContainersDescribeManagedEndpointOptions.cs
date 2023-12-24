using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-containers", "describe-managed-endpoint")]
public record AwsEmrContainersDescribeManagedEndpointOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--virtual-cluster-id")] string VirtualClusterId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}