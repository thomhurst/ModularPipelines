using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudcontrol", "update-resource")]
public record AwsCloudcontrolUpdateResourceOptions(
[property: CliOption("--type-name")] string TypeName,
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--patch-document")] string PatchDocument
) : AwsOptions
{
    [CliOption("--type-version-id")]
    public string? TypeVersionId { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}