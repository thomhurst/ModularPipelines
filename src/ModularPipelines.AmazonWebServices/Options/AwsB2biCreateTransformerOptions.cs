using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("b2bi", "create-transformer")]
public record AwsB2biCreateTransformerOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--file-format")] string FileFormat,
[property: CliOption("--mapping-template")] string MappingTemplate,
[property: CliOption("--edi-type")] string EdiType
) : AwsOptions
{
    [CliOption("--sample-document")]
    public string? SampleDocument { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}