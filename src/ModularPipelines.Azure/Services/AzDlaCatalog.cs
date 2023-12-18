using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla")]
public class AzDlaCatalog
{
    public AzDlaCatalog(
        AzDlaCatalogAssembly assembly,
        AzDlaCatalogCredential credential,
        AzDlaCatalogDatabase database,
        AzDlaCatalogExternalDataSource externalDataSource,
        AzDlaCatalogPackage package,
        AzDlaCatalogProcedure procedure,
        AzDlaCatalogSchema schema,
        AzDlaCatalogTable table,
        AzDlaCatalogTablePartition tablePartition,
        AzDlaCatalogTableStats tableStats,
        AzDlaCatalogTableType tableType,
        AzDlaCatalogTvf tvf,
        AzDlaCatalogView view
    )
    {
        Assembly = assembly;
        Credential = credential;
        Database = database;
        ExternalDataSource = externalDataSource;
        Package = package;
        Procedure = procedure;
        Schema = schema;
        Table = table;
        TablePartition = tablePartition;
        TableStats = tableStats;
        TableType = tableType;
        Tvf = tvf;
        View = view;
    }

    public AzDlaCatalogAssembly Assembly { get; }

    public AzDlaCatalogCredential Credential { get; }

    public AzDlaCatalogDatabase Database { get; }

    public AzDlaCatalogExternalDataSource ExternalDataSource { get; }

    public AzDlaCatalogPackage Package { get; }

    public AzDlaCatalogProcedure Procedure { get; }

    public AzDlaCatalogSchema Schema { get; }

    public AzDlaCatalogTable Table { get; }

    public AzDlaCatalogTablePartition TablePartition { get; }

    public AzDlaCatalogTableStats TableStats { get; }

    public AzDlaCatalogTableType TableType { get; }

    public AzDlaCatalogTvf Tvf { get; }

    public AzDlaCatalogView View { get; }
}

