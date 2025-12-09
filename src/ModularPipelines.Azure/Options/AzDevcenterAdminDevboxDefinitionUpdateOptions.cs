using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "devbox-definition", "update")]
public record AzDevcenterAdminDevboxDefinitionUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--devbox-definition-name")]
    public string? DevboxDefinitionName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--hibernate-support")]
    public string? HibernateSupport { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--image-reference")]
    public string? ImageReference { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--os-storage-type")]
    public string? OsStorageType { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}