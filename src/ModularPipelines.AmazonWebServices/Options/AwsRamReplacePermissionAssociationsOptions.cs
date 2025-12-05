using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "replace-permission-associations")]
public record AwsRamReplacePermissionAssociationsOptions(
[property: CliOption("--from-permission-arn")] string FromPermissionArn,
[property: CliOption("--to-permission-arn")] string ToPermissionArn
) : AwsOptions
{
    [CliOption("--from-permission-version")]
    public int? FromPermissionVersion { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}