using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "get-template-sync-status")]
public record AwsProtonGetTemplateSyncStatusOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--template-type")] string TemplateType,
[property: CliOption("--template-version")] string TemplateVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}