using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplify", "update-domain-association")]
public record AwsAmplifyUpdateDomainAssociationOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--sub-domain-settings")]
    public string[]? SubDomainSettings { get; set; }

    [CommandSwitch("--auto-sub-domain-creation-patterns")]
    public string[]? AutoSubDomainCreationPatterns { get; set; }

    [CommandSwitch("--auto-sub-domain-iam-role")]
    public string? AutoSubDomainIamRole { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}