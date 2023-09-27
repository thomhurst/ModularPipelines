namespace ModularPipelines.Helpers;

internal interface IPipelineDisposer
{
    Task DisposeAsync();
}