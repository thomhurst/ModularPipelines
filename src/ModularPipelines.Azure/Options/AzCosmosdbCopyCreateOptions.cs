using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "copy", "create")]
public record AzCosmosdbCopyCreateOptions(
[property: CliOption("--dest-account")] int DestAccount,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--src-account")] int SrcAccount
) : AzOptions
{
    [CliOption("--dest-cassandra")]
    public string? DestCassandra { get; set; }

    [CliOption("--dest-mongo")]
    public string? DestMongo { get; set; }

    [CliOption("--dest-nosql")]
    public string? DestNosql { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--src-cassandra")]
    public string? SrcCassandra { get; set; }

    [CliOption("--src-mongo")]
    public string? SrcMongo { get; set; }

    [CliOption("--src-nosql")]
    public string? SrcNosql { get; set; }
}