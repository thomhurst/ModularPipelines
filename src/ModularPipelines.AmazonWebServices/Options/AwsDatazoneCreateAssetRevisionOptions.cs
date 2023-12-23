using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "create-asset-revision")]
public record AwsDatazoneCreateAssetRevisionOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--forms-input")]
    public string[]? FormsInput { get; set; }

    [CommandSwitch("--glossary-terms")]
    public string[]? GlossaryTerms { get; set; }

    [CommandSwitch("--prediction-configuration")]
    public string? PredictionConfiguration { get; set; }

    [CommandSwitch("--type-revision")]
    public string? TypeRevision { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}