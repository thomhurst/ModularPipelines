using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-document-default-version")]
public record AwsSsmUpdateDocumentDefaultVersionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--document-version")] string DocumentVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}