using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "create-cluster")]
public record AwsEcsCreateClusterOptions : AwsOptions
{
    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--settings")]
    public string[]? Settings { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--capacity-providers")]
    public string[]? CapacityProviders { get; set; }

    [CommandSwitch("--default-capacity-provider-strategy")]
    public string[]? DefaultCapacityProviderStrategy { get; set; }

    [CommandSwitch("--service-connect-defaults")]
    public string? ServiceConnectDefaults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}