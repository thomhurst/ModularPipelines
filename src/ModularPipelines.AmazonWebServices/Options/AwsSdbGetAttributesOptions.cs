using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sdb", "get-attributes")]
public record AwsSdbGetAttributesOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--item-name")] string ItemName
) : AwsOptions
{
    [CliOption("--attribute-names")]
    public string[]? AttributeNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}