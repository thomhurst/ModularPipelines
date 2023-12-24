using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "update-configuration-profile")]
public record AwsAppconfigUpdateConfigurationProfileOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--configuration-profile-id")] string ConfigurationProfileId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--retrieval-role-arn")]
    public string? RetrievalRoleArn { get; set; }

    [CommandSwitch("--validators")]
    public string[]? Validators { get; set; }

    [CommandSwitch("--kms-key-identifier")]
    public string? KmsKeyIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}