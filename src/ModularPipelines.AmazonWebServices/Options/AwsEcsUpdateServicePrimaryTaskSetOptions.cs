using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "update-service-primary-task-set")]
public record AwsEcsUpdateServicePrimaryTaskSetOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--service")] string Service,
[property: CliOption("--primary-task-set")] string PrimaryTaskSet
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}