using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "put-signing-profile")]
public record AwsSignerPutSigningProfileOptions(
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--platform-id")] string PlatformId
) : AwsOptions
{
    [CliOption("--signing-material")]
    public string? SigningMaterial { get; set; }

    [CliOption("--signature-validity-period")]
    public string? SignatureValidityPeriod { get; set; }

    [CliOption("--overrides")]
    public string? Overrides { get; set; }

    [CliOption("--signing-parameters")]
    public IEnumerable<KeyValue>? SigningParameters { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}