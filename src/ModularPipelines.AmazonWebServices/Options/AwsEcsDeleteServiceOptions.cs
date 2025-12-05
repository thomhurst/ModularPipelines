using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "delete-service")]
public record AwsEcsDeleteServiceOptions(
[property: CliOption("--service")] string Service
) : AwsOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}