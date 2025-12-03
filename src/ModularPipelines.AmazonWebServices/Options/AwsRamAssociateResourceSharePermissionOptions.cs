using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "associate-resource-share-permission")]
public record AwsRamAssociateResourceSharePermissionOptions(
[property: CliOption("--resource-share-arn")] string ResourceShareArn,
[property: CliOption("--permission-arn")] string PermissionArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--permission-version")]
    public int? PermissionVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}