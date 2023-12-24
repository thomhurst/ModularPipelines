using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "associate-encryption-config")]
public record AwsEksAssociateEncryptionConfigOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--encryption-config")] string[] EncryptionConfig
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}