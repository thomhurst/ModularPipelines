using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identitystore", "update-user")]
public record AwsIdentitystoreUpdateUserOptions(
[property: CommandSwitch("--identity-store-id")] string IdentityStoreId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--operations")] string[] Operations
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}