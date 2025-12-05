using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "update-key-value-store")]
public record AwsCloudfrontUpdateKeyValueStoreOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--comment")] string Comment,
[property: CliOption("--if-match")] string IfMatch
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}