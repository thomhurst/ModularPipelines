using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearchdomain", "suggest")]
public record AwsCloudsearchdomainSuggestOptions(
[property: CliOption("--suggester")] string Suggester,
[property: CliOption("--suggest-query")] string SuggestQuery
) : AwsOptions
{
    [CliOption("--size")]
    public long? Size { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}