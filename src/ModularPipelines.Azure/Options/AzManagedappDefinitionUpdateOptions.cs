using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedapp", "definition", "update")]
public record AzManagedappDefinitionUpdateOptions(
[property: CliOption("--authorizations")] string Authorizations,
[property: CliOption("--description")] string Description,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--lock-level")] string LockLevel
) : AzOptions
{
    [CliOption("--create-ui-definition")]
    public string? CreateUiDefinition { get; set; }

    [CliOption("--deployment-mode")]
    public string? DeploymentMode { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--main-template")]
    public string? MainTemplate { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--package-file-uri")]
    public string? PackageFileUri { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}