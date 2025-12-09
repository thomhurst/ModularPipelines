using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("b2bi", "update-transformer")]
public record AwsB2biUpdateTransformerOptions(
[property: CliOption("--transformer-id")] string TransformerId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--file-format")]
    public string? FileFormat { get; set; }

    [CliOption("--mapping-template")]
    public string? MappingTemplate { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--edi-type")]
    public string? EdiType { get; set; }

    [CliOption("--sample-document")]
    public string? SampleDocument { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}