using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-security-configuration")]
public record AwsGlueCreateSecurityConfigurationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--encryption-configuration")] string EncryptionConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}