using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "image-definition", "create")]
public record AzSigImageDefinitionCreateOptions(
[property: CliOption("--gallery-image-definition")] string GalleryImageDefinition,
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--offer")] string Offer,
[property: CliOption("--os-type")] string OsType,
[property: CliOption("--publisher")] string Publisher,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--disallowed-disk-types")]
    public string? DisallowedDiskTypes { get; set; }

    [CliOption("--end-of-life-date")]
    public string? EndOfLifeDate { get; set; }

    [CliOption("--eula")]
    public string? Eula { get; set; }

    [CliOption("--features")]
    public string? Features { get; set; }

    [CliOption("--hyper-v-generation")]
    public string? HyperVGeneration { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--maximum-cpu-core")]
    public string? MaximumCpuCore { get; set; }

    [CliOption("--maximum-memory")]
    public string? MaximumMemory { get; set; }

    [CliOption("--minimum-cpu-core")]
    public string? MinimumCpuCore { get; set; }

    [CliOption("--minimum-memory")]
    public string? MinimumMemory { get; set; }

    [CliOption("--os-state")]
    public string? OsState { get; set; }

    [CliOption("--plan-name")]
    public string? PlanName { get; set; }

    [CliOption("--plan-product")]
    public string? PlanProduct { get; set; }

    [CliOption("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CliOption("--privacy-statement-uri")]
    public string? PrivacyStatementUri { get; set; }

    [CliOption("--release-note-uri")]
    public string? ReleaseNoteUri { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}