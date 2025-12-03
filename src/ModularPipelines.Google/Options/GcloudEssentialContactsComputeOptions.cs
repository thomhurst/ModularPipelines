using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("essential-contacts", "compute")]
public record GcloudEssentialContactsComputeOptions(
[property: CliOption("--notification-categories")] string[] NotificationCategories
) : GcloudOptions
{
    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliOption("--project")]
    public new string? Project { get; set; }
}