using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("trustedadvisor", "list-checks")]
public record AwsTrustedadvisorListChecksOptions : AwsOptions
{
    [CliOption("--aws-service")]
    public string? AwsService { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--pillar")]
    public string? Pillar { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}