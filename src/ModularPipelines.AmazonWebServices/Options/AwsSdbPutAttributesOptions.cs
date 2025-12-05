using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sdb", "put-attributes")]
public record AwsSdbPutAttributesOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--item-name")] string ItemName,
[property: CliOption("--attributes")] string[] Attributes
) : AwsOptions
{
    [CliOption("--expected")]
    public string? Expected { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}