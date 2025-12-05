using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("oam", "update-link")]
public record AwsOamUpdateLinkOptions(
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--resource-types")] string[] ResourceTypes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}