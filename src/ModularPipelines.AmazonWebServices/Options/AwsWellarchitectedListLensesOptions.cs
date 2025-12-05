using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "list-lenses")]
public record AwsWellarchitectedListLensesOptions : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--lens-type")]
    public string? LensType { get; set; }

    [CliOption("--lens-status")]
    public string? LensStatus { get; set; }

    [CliOption("--lens-name")]
    public string? LensName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}