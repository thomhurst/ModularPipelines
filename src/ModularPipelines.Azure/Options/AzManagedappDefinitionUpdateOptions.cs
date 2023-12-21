using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedapp", "definition", "update")]
public record AzManagedappDefinitionUpdateOptions(
[property: CommandSwitch("--authorizations")] string Authorizations,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--lock-level")] string LockLevel
) : AzOptions
{
    [CommandSwitch("--create-ui-definition")]
    public string? CreateUiDefinition { get; set; }

    [CommandSwitch("--deployment-mode")]
    public string? DeploymentMode { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--main-template")]
    public string? MainTemplate { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--package-file-uri")]
    public string? PackageFileUri { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}