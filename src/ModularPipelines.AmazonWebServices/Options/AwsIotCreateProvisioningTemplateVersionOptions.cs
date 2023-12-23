using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-provisioning-template-version")]
public record AwsIotCreateProvisioningTemplateVersionOptions(
[property: CommandSwitch("--template-name")] string TemplateName,
[property: CommandSwitch("--template-body")] string TemplateBody
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}