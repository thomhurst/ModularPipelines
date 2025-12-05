using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "update-cluster-settings")]
public record AwsEcsUpdateClusterSettingsOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--settings")] string[] Settings
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}