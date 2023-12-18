using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "copy", "cancel")]
public record AzCosmosdbCopyCancelOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--dest-cassandra")]
    public string? DestCassandra { get; set; }

    [CommandSwitch("--dest-mongo")]
    public string? DestMongo { get; set; }

    [CommandSwitch("--dest-nosql")]
    public string? DestNosql { get; set; }

    [CommandSwitch("--src-cassandra")]
    public string? SrcCassandra { get; set; }

    [CommandSwitch("--src-mongo")]
    public string? SrcMongo { get; set; }

    [CommandSwitch("--src-nosql")]
    public string? SrcNosql { get; set; }
}