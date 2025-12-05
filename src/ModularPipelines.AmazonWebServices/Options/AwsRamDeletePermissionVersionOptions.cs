using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "delete-permission-version")]
public record AwsRamDeletePermissionVersionOptions(
[property: CliOption("--permission-arn")] string PermissionArn,
[property: CliOption("--permission-version")] int PermissionVersion
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}