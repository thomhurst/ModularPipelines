using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Reporting;
using Spectre.Console;

namespace ModularPipelines.PipelineCli;

internal sealed class PipelineCommandHandler(
    PipelineCommandLineOptions commandLineOptions,
    IEnumerable<IModule> modules,
    IDependencyChainProvider dependencyChainProvider,
    IRegistrationEventExecutor registrationEventExecutor,
    IModuleDependencyRegistry dependencyRegistry,
    IModuleMetadataRegistry metadataRegistry,
    IDependencyGraphExporter dependencyGraphExporter,
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
            case PipelineCommand.DryRun:
                return null;
            case PipelineCommand.ListModules:
                await FinalizeModulesAsync(cancellationToken).ConfigureAwait(false);
                return ListModules();
            case PipelineCommand.Validate:
                await FinalizeModulesAsync(cancellationToken).ConfigureAwait(false);
                return ReportSuccessfulValidation();
            case PipelineCommand.ExportGraph:
                await ExportGraphAsync(cancellationToken).ConfigureAwait(false);
                return CreateSummary();
            default:
                throw new ArgumentOutOfRangeException(nameof(commandLineOptions));
        }
    }

    private async Task ExportGraphAsync(CancellationToken cancellationToken)
    {
        await dependencyGraphExporter.ExportAsync(
                commandLineOptions.GraphFormat
                ?? throw new InvalidOperationException("A dependency graph format is required."),
                commandLineOptions.GraphPath
                ?? throw new InvalidOperationException("A dependency graph path is required."),
                cancellationToken)
            .ConfigureAwait(false);
        consoleWriter.LogToConsole(
            $"Dependency graph written to {Path.GetFullPath(commandLineOptions.GraphPath)}");
    }

    private PipelineSummary ListModules()
    {
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
                Markup.Escape(metadataRegistry.GetCategory(moduleType) ?? string.Empty),
                Markup.Escape(string.Join(", ", dependencies)));
        }

        consoleWriter.Write(table);
        return CreateSummary();
    }

    private async Task FinalizeModulesAsync(CancellationToken cancellationToken)
    {
        await registrationEventExecutor.InvokeRegistrationEventsAsync(_modules).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        ModuleDependencyValidator.Validate(_modules, dependencyRegistry, metadataRegistry);
        dependencyChainProvider.Initialize(_modules);
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
            now)
        {
            StatusOverride = ModuleStatus.Succeeded,
        };
    }
}
