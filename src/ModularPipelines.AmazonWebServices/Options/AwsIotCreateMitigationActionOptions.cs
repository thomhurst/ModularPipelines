using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-mitigation-action")]
public record AwsIotCreateMitigationActionOptions(
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--action-params")] string ActionParams
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}