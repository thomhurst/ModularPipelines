using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "update-model")]
public record AwsLookoutequipmentUpdateModelOptions(
[property: CliOption("--model-name")] string ModelName
) : AwsOptions
{
    [CliOption("--labels-input-configuration")]
    public string? LabelsInputConfiguration { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}