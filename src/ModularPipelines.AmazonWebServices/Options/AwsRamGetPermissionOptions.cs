using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "get-permission")]
public record AwsRamGetPermissionOptions(
[property: CliOption("--permission-arn")] string PermissionArn
) : AwsOptions
{
    [CliOption("--permission-version")]
    public int? PermissionVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}