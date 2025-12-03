using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedapp", "definition", "create")]
public record AzManagedappDefinitionCreateOptions(
[property: CliOption("--authorizations")] string Authorizations,
[property: CliOption("--description")] string Description,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--lock-level")] string LockLevel,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--create-ui-definition")]
    public string? CreateUiDefinition { get; set; }

    [CliOption("--deployment-mode")]
    public string? DeploymentMode { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--main-template")]
    public string? MainTemplate { get; set; }

    [CliOption("--package-file-uri")]
    public string? PackageFileUri { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}