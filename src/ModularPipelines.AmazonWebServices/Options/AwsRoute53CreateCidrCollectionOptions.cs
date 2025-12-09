using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "create-cidr-collection")]
public record AwsRoute53CreateCidrCollectionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--caller-reference")] string CallerReference
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}