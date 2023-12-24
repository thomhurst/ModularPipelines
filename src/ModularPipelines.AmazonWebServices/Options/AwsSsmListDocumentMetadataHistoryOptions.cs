using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "list-document-metadata-history")]
public record AwsSsmListDocumentMetadataHistoryOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--metadata")] string Metadata
) : AwsOptions
{
    [CommandSwitch("--document-version")]
    public string? DocumentVersion { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}