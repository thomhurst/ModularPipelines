using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "copy", "create")]
public record AzCosmosdbCopyCreateOptions(
[property: CommandSwitch("--dest-account")] int DestAccount,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--src-account")] int SrcAccount
) : AzOptions
{
    [CommandSwitch("--dest-cassandra")]
    public string? DestCassandra { get; set; }

    [CommandSwitch("--dest-mongo")]
    public string? DestMongo { get; set; }

    [CommandSwitch("--dest-nosql")]
    public string? DestNosql { get; set; }

    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--src-cassandra")]
    public string? SrcCassandra { get; set; }

    [CommandSwitch("--src-mongo")]
    public string? SrcMongo { get; set; }

    [CommandSwitch("--src-nosql")]
    public string? SrcNosql { get; set; }
}