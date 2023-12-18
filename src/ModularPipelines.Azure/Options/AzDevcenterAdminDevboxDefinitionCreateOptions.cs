using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "devbox-definition", "create")]
public record AzDevcenterAdminDevboxDefinitionCreateOptions(
[property: CommandSwitch("--dev-center")] string DevCenter,
[property: CommandSwitch("--devbox-definition-name")] string DevboxDefinitionName,
[property: CommandSwitch("--image-reference")] string ImageReference,
[property: CommandSwitch("--os-storage-type")] string OsStorageType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--hibernate-support")]
    public string? HibernateSupport { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}