using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("proton", "get-template-sync-config")]
public record AwsProtonGetTemplateSyncConfigOptions(
[property: CliOption("--template-name")] string TemplateName,
[property: CliOption("--template-type")] string TemplateType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}