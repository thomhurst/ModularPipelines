using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "create-cluster")]
public record AwsEcsCreateClusterOptions : AwsOptions
{
    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--settings")]
    public string[]? Settings { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--capacity-providers")]
    public string[]? CapacityProviders { get; set; }

    [CliOption("--default-capacity-provider-strategy")]
    public string[]? DefaultCapacityProviderStrategy { get; set; }

    [CliOption("--service-connect-defaults")]
    public string? ServiceConnectDefaults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}