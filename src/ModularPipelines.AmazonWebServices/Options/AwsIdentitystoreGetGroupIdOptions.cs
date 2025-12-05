using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identitystore", "get-group-id")]
public record AwsIdentitystoreGetGroupIdOptions(
[property: CliOption("--identity-store-id")] string IdentityStoreId,
[property: CliOption("--alternate-identifier")] string AlternateIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}