using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "revoke-grant")]
public record AwsKmsRevokeGrantOptions(
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--grant-id")] string GrantId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}