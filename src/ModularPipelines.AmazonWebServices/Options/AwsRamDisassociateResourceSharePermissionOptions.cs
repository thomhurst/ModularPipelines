using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "disassociate-resource-share-permission")]
public record AwsRamDisassociateResourceSharePermissionOptions(
[property: CliOption("--resource-share-arn")] string ResourceShareArn,
[property: CliOption("--permission-arn")] string PermissionArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}