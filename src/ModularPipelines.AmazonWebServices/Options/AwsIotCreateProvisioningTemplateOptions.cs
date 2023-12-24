using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-provisioning-template")]
public record AwsIotCreateProvisioningTemplateOptions(
[property: CommandSwitch("--template-name")] string TemplateName,
[property: CommandSwitch("--template-body")] string TemplateBody,
[property: CommandSwitch("--provisioning-role-arn")] string ProvisioningRoleArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--pre-provisioning-hook")]
    public string? PreProvisioningHook { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}