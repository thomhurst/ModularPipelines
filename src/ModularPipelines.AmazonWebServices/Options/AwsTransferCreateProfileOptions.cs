using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "create-profile")]
public record AwsTransferCreateProfileOptions(
[property: CommandSwitch("--as2-id")] string As2Id,
[property: CommandSwitch("--profile-type")] string ProfileType
) : AwsOptions
{
    [CommandSwitch("--certificate-ids")]
    public string[]? CertificateIds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}