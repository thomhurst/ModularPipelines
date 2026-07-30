using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Spectre.Console;

namespace ModularPipelines.PipelineCli;

internal sealed class PipelineCommandHandler(
    PipelineCommandLineOptions commandLineOptions,
    IEnumerable<IModule> modules,
    IDependencyChainProvider dependencyChainProvider,
    IRegistrationEventExecutor registrationEventExecutor,
    IConsoleWriter consoleWriter)
{
    private readonly IReadOnlyList<IModule> _modules = modules
        .Distinct<IModule>(ReferenceEqualityComparer.Instance)
        .ToArray();

    public async Task<PipelineSummary?> TryExecuteAsync(CancellationToken cancellationToken)
    {
        switch (commandLineOptions.Command)
        {
            case PipelineCommand.Run:
                return null;
            case PipelineCommand.ListModules:
                await registrationEventExecutor.InvokeRegistrationEventsAsync(_modules).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                return ListModules();
            case PipelineCommand.Validate:
                return ReportSuccessfulValidation();
            default:
                throw new ArgumentOutOfRangeException(nameof(commandLineOptions));
        }
    }

    private PipelineSummary ListModules()
    {
        dependencyChainProvider.Initialize(_modules);
        var table = new Table
        {
            Border = TableBorder.Rounded,
            Title = new TableTitle("[bold]Pipeline modules[/]"),
        };
        table.AddColumn("[bold]Module[/]");
        table.AddColumn("[bold]Category[/]");
        table.AddColumn("[bold]Dependencies[/]");

        foreach (var model in dependencyChainProvider.ModuleDependencyModels
                     .OrderBy(model => model.Module.GetType().FullName, StringComparer.Ordinal))
        {
            var moduleType = model.Module.GetType();
            var dependencies = model.IsDependentOn
                .Select(dependency => dependency.Module.GetType().Name)
                .Order(StringComparer.Ordinal)
                .ToArray();
            table.AddRow(
                Markup.Escape(moduleType.FullName ?? moduleType.Name),
                Markup.Escape(model.Module.Configuration.Category ?? string.Empty),
                Markup.Escape(string.Join(", ", dependencies)));
        }

        consoleWriter.Write(table);
        return CreateSummary();
    }

    private PipelineSummary ReportSuccessfulValidation()
    {
        consoleWriter.LogToConsole("[green]Pipeline validation succeeded.[/]");
        return CreateSummary();
    }

    private PipelineSummary CreateSummary()
    {
        var now = DateTimeOffset.UtcNow;
        return new PipelineSummary(
            _modules,
            [],
            TimeSpan.Zero,
            now,
            now);
    }
}
