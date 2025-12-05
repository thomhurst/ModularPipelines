using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "update-domain-association")]
public record AwsAmplifyUpdateDomainAssociationOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--sub-domain-settings")]
    public string[]? SubDomainSettings { get; set; }

    [CliOption("--auto-sub-domain-creation-patterns")]
    public string[]? AutoSubDomainCreationPatterns { get; set; }

    [CliOption("--auto-sub-domain-iam-role")]
    public string? AutoSubDomainIamRole { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}