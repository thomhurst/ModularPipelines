using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-containers", "list-virtual-clusters")]
public record AwsEmrContainersListVirtualClustersOptions : AwsOptions
{
    [CommandSwitch("--container-provider-id")]
    public string? ContainerProviderId { get; set; }

    [CommandSwitch("--container-provider-type")]
    public string? ContainerProviderType { get; set; }

    [CommandSwitch("--created-after")]
    public long? CreatedAfter { get; set; }

    [CommandSwitch("--created-before")]
    public long? CreatedBefore { get; set; }

    [CommandSwitch("--states")]
    public string[]? States { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}