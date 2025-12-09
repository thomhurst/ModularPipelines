using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "create-profile")]
public record AwsTransferCreateProfileOptions(
[property: CliOption("--as2-id")] string As2Id,
[property: CliOption("--profile-type")] string ProfileType
) : AwsOptions
{
    [CliOption("--certificate-ids")]
    public string[]? CertificateIds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}