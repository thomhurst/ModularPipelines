using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identitystore", "delete-group")]
public record AwsIdentitystoreDeleteGroupOptions(
[property: CliOption("--identity-store-id")] string IdentityStoreId,
[property: CliOption("--group-id")] string GroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}