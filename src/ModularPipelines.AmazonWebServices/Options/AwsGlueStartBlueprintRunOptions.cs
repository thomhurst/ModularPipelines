using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "start-blueprint-run")]
public record AwsGlueStartBlueprintRunOptions(
[property: CliOption("--blueprint-name")] string BlueprintName,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--parameters")]
    public string? Parameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}