using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing")]
public class AzBillingInvoice
{
    public AzBillingInvoice(
        AzBillingInvoiceSection section,
        ICommand internalCommand
    )
    {
        Section = section;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBillingInvoiceSection Section { get; }

    public async Task<CommandResult> Download(AzBillingInvoiceDownloadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBillingInvoiceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBillingInvoiceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}