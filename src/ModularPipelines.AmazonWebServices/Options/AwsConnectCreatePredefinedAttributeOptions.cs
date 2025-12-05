using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-predefined-attribute")]
public record AwsConnectCreatePredefinedAttributeOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--values")] string Values
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}