using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifyuibuilder", "create-component")]
public record AwsAmplifyuibuilderCreateComponentOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--component-to-create")] string ComponentToCreate
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}