using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "update-configuration-profile")]
public record AwsAppconfigUpdateConfigurationProfileOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--configuration-profile-id")] string ConfigurationProfileId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--retrieval-role-arn")]
    public string? RetrievalRoleArn { get; set; }

    [CliOption("--validators")]
    public string[]? Validators { get; set; }

    [CliOption("--kms-key-identifier")]
    public string? KmsKeyIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}