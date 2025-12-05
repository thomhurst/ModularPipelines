using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifyuibuilder", "export-themes")]
public record AwsAmplifyuibuilderExportThemesOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--environment-name")] string EnvironmentName
) : AwsOptions
{
    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}