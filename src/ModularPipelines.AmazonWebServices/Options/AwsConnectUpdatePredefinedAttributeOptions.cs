using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-predefined-attribute")]
public record AwsConnectUpdatePredefinedAttributeOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--values")]
    public string? Values { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}