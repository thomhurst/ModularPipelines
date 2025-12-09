using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "create-data-source")]
public record AwsDatazoneCreateDataSourceOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--environment-identifier")] string EnvironmentIdentifier,
[property: CliOption("--name")] string Name,
[property: CliOption("--project-identifier")] string ProjectIdentifier,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--asset-forms-input")]
    public string[]? AssetFormsInput { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--enable-setting")]
    public string? EnableSetting { get; set; }

    [CliOption("--recommendation")]
    public string? Recommendation { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}