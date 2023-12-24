using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "batch-put-document")]
public record AwsKendraBatchPutDocumentOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--documents")] string[] Documents
) : AwsOptions
{
    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--custom-document-enrichment-configuration")]
    public string? CustomDocumentEnrichmentConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}