using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("voice-id", "update-domain")]
public record AwsVoiceIdUpdateDomainOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--name")] string Name,
[property: CliOption("--server-side-encryption-configuration")] string ServerSideEncryptionConfiguration
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}