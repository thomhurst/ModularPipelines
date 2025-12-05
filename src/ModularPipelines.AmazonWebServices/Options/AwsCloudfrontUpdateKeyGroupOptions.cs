using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "update-key-group")]
public record AwsCloudfrontUpdateKeyGroupOptions(
[property: CliOption("--key-group-config")] string KeyGroupConfig,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}