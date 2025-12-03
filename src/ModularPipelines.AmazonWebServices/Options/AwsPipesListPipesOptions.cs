using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipes", "list-pipes")]
public record AwsPipesListPipesOptions : AwsOptions
{
    [CliOption("--current-state")]
    public string? CurrentState { get; set; }

    [CliOption("--desired-state")]
    public string? DesiredState { get; set; }

    [CliOption("--name-prefix")]
    public string? NamePrefix { get; set; }

    [CliOption("--source-prefix")]
    public string? SourcePrefix { get; set; }

    [CliOption("--target-prefix")]
    public string? TargetPrefix { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}