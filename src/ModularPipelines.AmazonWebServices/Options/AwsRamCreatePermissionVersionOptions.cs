using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ram", "create-permission-version")]
public record AwsRamCreatePermissionVersionOptions(
[property: CliOption("--permission-arn")] string PermissionArn,
[property: CliOption("--policy-template")] string PolicyTemplate
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}