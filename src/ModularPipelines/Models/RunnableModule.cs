using ModularPipelines.Modules;

namespace ModularPipelines.Models;

internal record RunnableModule(ModuleBase Module, TimeSpan EstimatedDuration, ICollection<SubModuleEstimation> SubModuleEstimations);