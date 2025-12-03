using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "associate-encryption-config")]
public record AwsEksAssociateEncryptionConfigOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--encryption-config")] string[] EncryptionConfig
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}