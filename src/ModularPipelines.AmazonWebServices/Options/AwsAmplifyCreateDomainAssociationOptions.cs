using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "create-domain-association")]
public record AwsAmplifyCreateDomainAssociationOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--sub-domain-settings")] string[] SubDomainSettings
) : AwsOptions
{
    [CliOption("--auto-sub-domain-creation-patterns")]
    public string[]? AutoSubDomainCreationPatterns { get; set; }

    [CliOption("--auto-sub-domain-iam-role")]
    public string? AutoSubDomainIamRole { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}