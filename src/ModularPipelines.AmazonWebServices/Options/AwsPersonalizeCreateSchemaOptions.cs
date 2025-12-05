using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("personalize", "create-schema")]
public record AwsPersonalizeCreateSchemaOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--schema")] string Schema
) : AwsOptions
{
    [CliOption("--domain")]
    public string? Domain { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}