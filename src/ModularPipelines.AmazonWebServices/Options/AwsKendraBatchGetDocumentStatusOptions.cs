using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "batch-get-document-status")]
public record AwsKendraBatchGetDocumentStatusOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--document-info-list")] string[] DocumentInfoList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}