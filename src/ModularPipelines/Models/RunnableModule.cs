using ModularPipelines.Modules;

namespace ModularPipelines.Models;

internal record RunnableModule(IModule Module, TimeSpan EstimatedDuration, ICollection<SubModuleEstimation> SubModuleEstimations);