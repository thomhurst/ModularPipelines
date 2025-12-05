using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearch", "define-suggester")]
public record AwsCloudsearchDefineSuggesterOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--suggester")] string Suggester
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}