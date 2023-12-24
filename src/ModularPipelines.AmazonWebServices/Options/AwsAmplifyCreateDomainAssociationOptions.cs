using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplify", "create-domain-association")]
public record AwsAmplifyCreateDomainAssociationOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--sub-domain-settings")] string[] SubDomainSettings
) : AwsOptions
{
    [CommandSwitch("--auto-sub-domain-creation-patterns")]
    public string[]? AutoSubDomainCreationPatterns { get; set; }

    [CommandSwitch("--auto-sub-domain-iam-role")]
    public string? AutoSubDomainIamRole { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}