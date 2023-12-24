using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "delete-template-sync-config")]
public record AwsProtonDeleteTemplateSyncConfigOptions(
[property: CommandSwitch("--template-name")] string TemplateName,
[property: CommandSwitch("--template-type")] string TemplateType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}