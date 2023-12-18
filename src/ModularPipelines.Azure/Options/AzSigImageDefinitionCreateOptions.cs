using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-definition", "create")]
public record AzSigImageDefinitionCreateOptions(
[property: CommandSwitch("--gallery-image-definition")] string GalleryImageDefinition,
[property: CommandSwitch("--gallery-name")] string GalleryName,
[property: CommandSwitch("--offer")] string Offer,
[property: CommandSwitch("--os-type")] string OsType,
[property: CommandSwitch("--publisher")] string Publisher,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--disallowed-disk-types")]
    public string? DisallowedDiskTypes { get; set; }

    [CommandSwitch("--end-of-life-date")]
    public string? EndOfLifeDate { get; set; }

    [CommandSwitch("--eula")]
    public string? Eula { get; set; }

    [CommandSwitch("--features")]
    public string? Features { get; set; }

    [CommandSwitch("--hyper-v-generation")]
    public string? HyperVGeneration { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--maximum-cpu-core")]
    public string? MaximumCpuCore { get; set; }

    [CommandSwitch("--maximum-memory")]
    public string? MaximumMemory { get; set; }

    [CommandSwitch("--minimum-cpu-core")]
    public string? MinimumCpuCore { get; set; }

    [CommandSwitch("--minimum-memory")]
    public string? MinimumMemory { get; set; }

    [CommandSwitch("--os-state")]
    public string? OsState { get; set; }

    [CommandSwitch("--plan-name")]
    public string? PlanName { get; set; }

    [CommandSwitch("--plan-product")]
    public string? PlanProduct { get; set; }

    [CommandSwitch("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CommandSwitch("--privacy-statement-uri")]
    public string? PrivacyStatementUri { get; set; }

    [CommandSwitch("--release-note-uri")]
    public string? ReleaseNoteUri { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}