using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifyuibuilder", "get-metadata")]
public record AwsAmplifyuibuilderGetMetadataOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--environment-name")] string EnvironmentName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}