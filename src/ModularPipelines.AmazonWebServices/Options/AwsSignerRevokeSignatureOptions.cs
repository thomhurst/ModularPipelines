using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signer", "revoke-signature")]
public record AwsSignerRevokeSignatureOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--reason")] string Reason
) : AwsOptions
{
    [CommandSwitch("--job-owner")]
    public string? JobOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}