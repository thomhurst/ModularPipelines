using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identitystore", "update-user")]
public record AwsIdentitystoreUpdateUserOptions(
[property: CliOption("--identity-store-id")] string IdentityStoreId,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--operations")] string[] Operations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}