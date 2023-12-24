using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-provisioning-template")]
public record AwsIotUpdateProvisioningTemplateOptions(
[property: CommandSwitch("--template-name")] string TemplateName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--default-version-id")]
    public int? DefaultVersionId { get; set; }

    [CommandSwitch("--provisioning-role-arn")]
    public string? ProvisioningRoleArn { get; set; }

    [CommandSwitch("--pre-provisioning-hook")]
    public string? PreProvisioningHook { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}