using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-asset")]
public record AwsDatazoneCreateAssetOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--name")] string Name,
[property: CliOption("--owning-project-identifier")] string OwningProjectIdentifier,
[property: CliOption("--type-identifier")] string TypeIdentifier
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--external-identifier")]
    public string? ExternalIdentifier { get; set; }

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