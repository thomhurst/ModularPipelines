using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "create-core-definition-version")]
public record AwsGreengrassCreateCoreDefinitionVersionOptions(
[property: CliOption("--core-definition-id")] string CoreDefinitionId
) : AwsOptions
{
    [CliOption("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CliOption("--cores")]
    public string[]? Cores { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}