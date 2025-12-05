using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudcontrol", "get-resource")]
public record AwsCloudcontrolGetResourceOptions(
[property: CliOption("--type-name")] string TypeName,
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--type-version-id")]
    public string? TypeVersionId { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}