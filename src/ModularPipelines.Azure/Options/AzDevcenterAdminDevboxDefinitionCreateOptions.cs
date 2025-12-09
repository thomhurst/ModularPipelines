using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "devbox-definition", "create")]
public record AzDevcenterAdminDevboxDefinitionCreateOptions(
[property: CliOption("--dev-center")] string DevCenter,
[property: CliOption("--devbox-definition-name")] string DevboxDefinitionName,
[property: CliOption("--image-reference")] string ImageReference,
[property: CliOption("--os-storage-type")] string OsStorageType,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--hibernate-support")]
    public string? HibernateSupport { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}