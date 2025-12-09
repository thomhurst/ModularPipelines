using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "get-task-protection")]
public record AwsEcsGetTaskProtectionOptions(
[property: CliOption("--cluster")] string Cluster
) : AwsOptions
{
    [CliOption("--tasks")]
    public string[]? Tasks { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}