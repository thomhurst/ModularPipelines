using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sdb", "delete-attributes")]
public record AwsSdbDeleteAttributesOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--item-name")] string ItemName
) : AwsOptions
{
    [CliOption("--attributes")]
    public string[]? Attributes { get; set; }

    [CliOption("--expected")]
    public string? Expected { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}