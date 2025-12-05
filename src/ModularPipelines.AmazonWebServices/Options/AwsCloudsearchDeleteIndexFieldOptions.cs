using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearch", "delete-index-field")]
public record AwsCloudsearchDeleteIndexFieldOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--index-field-name")] string IndexFieldName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}