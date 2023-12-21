using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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