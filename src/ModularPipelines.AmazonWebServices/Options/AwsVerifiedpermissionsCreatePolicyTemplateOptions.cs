using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("verifiedpermissions", "create-policy-template")]
public record AwsVerifiedpermissionsCreatePolicyTemplateOptions(
[property: CommandSwitch("--policy-store-id")] string PolicyStoreId,
[property: CommandSwitch("--statement")] string Statement
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}