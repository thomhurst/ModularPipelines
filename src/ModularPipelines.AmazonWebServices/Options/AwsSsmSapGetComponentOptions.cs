using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-sap", "get-component")]
public record AwsSsmSapGetComponentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--component-id")] string ComponentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}