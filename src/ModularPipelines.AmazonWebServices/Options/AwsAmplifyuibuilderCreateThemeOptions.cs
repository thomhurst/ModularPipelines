using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifyuibuilder", "create-theme")]
public record AwsAmplifyuibuilderCreateThemeOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--theme-to-create")] string ThemeToCreate
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}