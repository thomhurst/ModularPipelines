using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-asset-revision")]
public record AwsDatazoneCreateAssetRevisionOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--forms-input")]
    public string[]? FormsInput { get; set; }

    [CliOption("--glossary-terms")]
    public string[]? GlossaryTerms { get; set; }

    [CliOption("--prediction-configuration")]
    public string? PredictionConfiguration { get; set; }

    [CliOption("--type-revision")]
    public string? TypeRevision { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}