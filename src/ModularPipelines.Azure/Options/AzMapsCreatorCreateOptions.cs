using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maps", "creator", "create")]
public record AzMapsCreatorCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--creator-name")] string CreatorName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-units")] string StorageUnits
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}