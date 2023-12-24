using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "create-data-source")]
public record AwsKendraCreateDataSourceOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--language-code")]
    public string? LanguageCode { get; set; }

    [CommandSwitch("--custom-document-enrichment-configuration")]
    public string? CustomDocumentEnrichmentConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}