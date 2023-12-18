using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedapp", "definition", "create")]
public record AzManagedappDefinitionCreateOptions(
[property: CommandSwitch("--authorizations")] string Authorizations,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--lock-level")] string LockLevel,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--create-ui-definition")]
    public string? CreateUiDefinition { get; set; }

    [CommandSwitch("--deployment-mode")]
    public string? DeploymentMode { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--main-template")]
    public string? MainTemplate { get; set; }

    [CommandSwitch("--package-file-uri")]
    public string? PackageFileUri { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}