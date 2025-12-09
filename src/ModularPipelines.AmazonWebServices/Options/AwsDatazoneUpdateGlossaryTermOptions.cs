using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "update-glossary-term")]
public record AwsDatazoneUpdateGlossaryTermOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--glossary-identifier")]
    public string? GlossaryIdentifier { get; set; }

    [CliOption("--long-description")]
    public string? LongDescription { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--short-description")]
    public string? ShortDescription { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--term-relations")]
    public string? TermRelations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}