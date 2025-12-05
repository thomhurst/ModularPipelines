using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pi", "get-dimension-key-details")]
public record AwsPiGetDimensionKeyDetailsOptions(
[property: CliOption("--service-type")] string ServiceType,
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--group")] string Group,
[property: CliOption("--group-identifier")] string GroupIdentifier
) : AwsOptions
{
    [CliOption("--requested-dimensions")]
    public string[]? RequestedDimensions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}