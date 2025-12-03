using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "update-task-protection")]
public record AwsEcsUpdateTaskProtectionOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--tasks")] string[] Tasks
) : AwsOptions
{
    [CliOption("--expires-in-minutes")]
    public int? ExpiresInMinutes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}