using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-document-metadata")]
public record AwsSsmUpdateDocumentMetadataOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--document-reviews")] string DocumentReviews
) : AwsOptions
{
    [CommandSwitch("--document-version")]
    public string? DocumentVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}