using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "get-template-sync-status")]
public record AwsProtonGetTemplateSyncStatusOptions(
[property: CommandSwitch("--template-name")] string TemplateName,
[property: CommandSwitch("--template-type")] string TemplateType,
[property: CommandSwitch("--template-version")] string TemplateVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}