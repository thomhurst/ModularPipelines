using Pipeline.NET.Context;

namespace Pipeline.NET.Interfaces;

public interface IPipelineGlobalSetup
{
    Task OnStartAsync(IModuleContext moduleContext);
    Task OnEndAsync(IModuleContext moduleContext);
}