using ModularPipelines.Helpers;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors;

internal class PipelineInitializer : IPipelineInitializer
{
    private readonly IDependencyDetector _dependencyDetector;
    private readonly IRequirementChecker _requirementsChecker;
    private readonly IModuleRetriever _moduleRetriever;
    private readonly IConsolePrinter _consolePrinter;
    private readonly IPipelineSetupExecutor _pipelineSetupExecutor;

    public PipelineInitializer(IConsolePrinter consolePrinter,
        IModuleRetriever moduleRetriever,
        IRequirementChecker requirementsChecker,
        IDependencyDetector dependencyDetector,
        IPipelineSetupExecutor pipelineSetupExecutor)
    {
        _consolePrinter = consolePrinter;
        _moduleRetriever = moduleRetriever;
        _requirementsChecker = requirementsChecker;
        _dependencyDetector = dependencyDetector;
        _pipelineSetupExecutor = pipelineSetupExecutor;
    }

    public async Task<OrganizedModules> Initialize()
    {
        _consolePrinter.PrintLogo();

        _dependencyDetector.Check();

        await _pipelineSetupExecutor.OnStartAsync();

        await _requirementsChecker.CheckRequirementsAsync();

        return await _moduleRetriever.GetOrganizedModules();
    }
}