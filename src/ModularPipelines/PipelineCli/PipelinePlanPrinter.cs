using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using Spectre.Console;

namespace ModularPipelines.PipelineCli;

internal sealed class PipelinePlanPrinter(IConsoleWriter consoleWriter)
{
    public void Print(PipelinePlan plan)
    {
        var table = new Table
        {
            Border = TableBorder.Rounded,
            Title = new TableTitle("[bold]Pipeline dry-run plan[/]"),
        };
        table.AddColumn("[bold]Wave[/]");
        table.AddColumn("[bold]Wave ETA[/]");
        table.AddColumn("[bold]Module[/]");
        table.AddColumn("[bold]Category[/]");
        table.AddColumn("[bold]Decision[/]");
        table.AddColumn("[bold]Estimate[/]");

        foreach (var wave in plan.Waves)
        {
            foreach (var module in wave.Modules)
            {
                var decision = module.SkipDecision is null
                    ? "[yellow]Unknown: requires module results[/]"
                    : module.ShouldSkip
                        ? $"[yellow]Skip: {Markup.Escape(module.SkipDecision.Reason ?? "No reason provided")}[/]"
                        : module.IsCacheCandidate
                            ? "[green]Run[/] [dim](cache candidate)[/]"
                            : "[green]Run[/]";
                table.AddRow(
                    wave.Number.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    Markup.Escape(wave.EstimatedDuration.ToDisplayString()),
                    Markup.Escape(module.ModuleName),
                    Markup.Escape(module.Category ?? string.Empty),
                    decision,
                    module.ShouldSkip ? "-" : Markup.Escape(module.EstimatedDuration.ToDisplayString()));
            }
        }

        var cacheNote = plan.Waves.SelectMany(wave => wave.Modules).Any(module => module.IsCacheCandidate)
            ? " [dim](cache hits may reduce actual duration)[/]"
            : string.Empty;
        table.Caption = new TableTitle(
            $"[bold]Estimated pipeline duration: {Markup.Escape(plan.EstimatedDuration.ToDisplayString())}[/]{cacheNote}");
        consoleWriter.Write(table);
    }
}
