using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "create-data-source")]
public record AwsKendraCreateDataSourceOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--language-code")]
    public string? LanguageCode { get; set; }

    [CliOption("--custom-document-enrichment-configuration")]
    public string? CustomDocumentEnrichmentConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}