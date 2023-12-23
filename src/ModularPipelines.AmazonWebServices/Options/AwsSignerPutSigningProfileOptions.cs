using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signer", "put-signing-profile")]
public record AwsSignerPutSigningProfileOptions(
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--platform-id")] string PlatformId
) : AwsOptions
{
    [CommandSwitch("--signing-material")]
    public string? SigningMaterial { get; set; }

    [CommandSwitch("--signature-validity-period")]
    public string? SignatureValidityPeriod { get; set; }

    [CommandSwitch("--overrides")]
    public string? Overrides { get; set; }

    [CommandSwitch("--signing-parameters")]
    public IEnumerable<KeyValue>? SigningParameters { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}