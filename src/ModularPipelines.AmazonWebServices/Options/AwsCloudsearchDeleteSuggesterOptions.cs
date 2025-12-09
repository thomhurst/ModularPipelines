using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearch", "delete-suggester")]
public record AwsCloudsearchDeleteSuggesterOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--suggester-name")] string SuggesterName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}