using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguru-security", "update-account-configuration")]
public record AwsCodeguruSecurityUpdateAccountConfigurationOptions(
[property: CommandSwitch("--encryption-config")] string EncryptionConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}