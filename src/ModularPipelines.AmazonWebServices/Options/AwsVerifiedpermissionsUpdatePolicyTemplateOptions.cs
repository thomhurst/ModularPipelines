using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("verifiedpermissions", "update-policy-template")]
public record AwsVerifiedpermissionsUpdatePolicyTemplateOptions(
[property: CommandSwitch("--policy-store-id")] string PolicyStoreId,
[property: CommandSwitch("--policy-template-id")] string PolicyTemplateId,
[property: CommandSwitch("--statement")] string Statement
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}