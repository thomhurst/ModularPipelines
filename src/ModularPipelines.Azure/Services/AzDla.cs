using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDla
{
    public AzDla(
        AzDlaAccount account,
        AzDlaCatalog catalog,
        AzDlaJob job
    )
    {
        Account = account;
        Catalog = catalog;
        Job = job;
    }

    public AzDlaAccount Account { get; }

    public AzDlaCatalog Catalog { get; }

    public AzDlaJob Job { get; }
}