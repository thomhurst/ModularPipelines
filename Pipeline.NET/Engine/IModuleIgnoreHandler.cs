using Pipeline.NET.Modules;

namespace Pipeline.NET.Engine;

public interface IModuleIgnoreHandler
{
    bool ShouldIgnore(IModule module);
}