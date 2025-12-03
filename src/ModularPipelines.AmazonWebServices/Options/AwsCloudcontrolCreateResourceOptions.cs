using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudcontrol", "create-resource")]
public record AwsCloudcontrolCreateResourceOptions(
[property: CliOption("--type-name")] string TypeName,
[property: CliOption("--desired-state")] string DesiredState
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