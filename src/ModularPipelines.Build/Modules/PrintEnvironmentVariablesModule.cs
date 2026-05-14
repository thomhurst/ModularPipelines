using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using Spectre.Console;

namespace ModularPipelines.Build.Modules;

public class PrintEnvironmentVariablesModule : Module<IDictionary<string, object>>
{
    protected override Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var environmentVariables = context.Environment.Variables.GetEnvironmentVariables()
            .OrderBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var table = new Table
        {
            Border = TableBorder.Rounded,
        };

        table.AddColumn(new TableColumn("[bold]Name[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Value[/]").LeftAligned());

        foreach (var environmentVariable in environmentVariables)
        {
            table.AddRow(
                Markup.Escape(environmentVariable.Key),
                Markup.Escape(environmentVariable.Value));
        }

        ((IConsoleWriter)context.Logger).Write(table);

        return Task.FromResult<IDictionary<string, object>?>(
            environmentVariables.ToDictionary(x => x.Key, x => (object)x.Value));
    }
}
