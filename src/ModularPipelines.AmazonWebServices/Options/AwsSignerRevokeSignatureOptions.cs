using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("signer", "revoke-signature")]
public record AwsSignerRevokeSignatureOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--reason")] string Reason
) : AwsOptions
{
    [CliOption("--job-owner")]
    public string? JobOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}