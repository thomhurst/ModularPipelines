using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "update-data-source")]
public record AwsDatazoneUpdateDataSourceOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--asset-forms-input")]
    public string[]? AssetFormsInput { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--enable-setting")]
    public string? EnableSetting { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--recommendation")]
    public string? Recommendation { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}