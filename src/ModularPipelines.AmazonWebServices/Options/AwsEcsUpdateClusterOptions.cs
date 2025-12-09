using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "update-cluster")]
public record AwsEcsUpdateClusterOptions(
[property: CliOption("--cluster")] string Cluster
) : AwsOptions
{
    [CliOption("--settings")]
    public string[]? Settings { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--service-connect-defaults")]
    public string? ServiceConnectDefaults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}