using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "indexes", "composite", "create")]
public record GcloudFirestoreIndexesCompositeCreateOptions(
[property: CommandSwitch("--field-config")] string[] FieldConfig,
[property: CommandSwitch("--collection-group")] string CollectionGroup,
[property: CommandSwitch("--database")] string Database
) : GcloudOptions
{
    [CommandSwitch("--api-scope")]
    public string? ApiScope { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--query-scope")]
    public string? QueryScope { get; set; }
}