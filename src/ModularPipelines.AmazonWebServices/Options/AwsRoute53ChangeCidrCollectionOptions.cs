using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "change-cidr-collection")]
public record AwsRoute53ChangeCidrCollectionOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--changes")] string[] Changes
) : AwsOptions
{
    [CliOption("--collection-version")]
    public long? CollectionVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}