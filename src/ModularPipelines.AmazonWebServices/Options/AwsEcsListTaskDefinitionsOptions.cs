using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecs", "list-task-definitions")]
public record AwsEcsListTaskDefinitionsOptions : AwsOptions
{
    [CliOption("--family-prefix")]
    public string? FamilyPrefix { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--sort")]
    public string? Sort { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}