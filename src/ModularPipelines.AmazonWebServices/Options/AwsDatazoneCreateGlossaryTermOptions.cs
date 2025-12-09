using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-glossary-term")]
public record AwsDatazoneCreateGlossaryTermOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--glossary-identifier")] string GlossaryIdentifier,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--long-description")]
    public string? LongDescription { get; set; }

    [CliOption("--short-description")]
    public string? ShortDescription { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--term-relations")]
    public string? TermRelations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}