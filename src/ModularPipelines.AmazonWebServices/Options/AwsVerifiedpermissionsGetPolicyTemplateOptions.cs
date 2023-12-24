using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("verifiedpermissions", "get-policy-template")]
public record AwsVerifiedpermissionsGetPolicyTemplateOptions(
[property: CommandSwitch("--policy-store-id")] string PolicyStoreId,
[property: CommandSwitch("--policy-template-id")] string PolicyTemplateId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}