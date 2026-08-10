namespace ModularPipelines.Exceptions;

internal sealed class NoModulesRegisteredException()
    : PipelineException("No modules have been registered");
