using System.Security.Cryptography;
using System.Text;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal static class PipelineRunCompatibility
{
    public static string GetPipelineIdentity(string? configuredIdentity, IReadOnlyList<IModule> modules)
    {
        if (!string.IsNullOrWhiteSpace(configuredIdentity))
        {
            return configuredIdentity;
        }

        return Hash(string.Join('\n', modules
            .Select(static module => ModuleId.FromType(module.GetType()).Value)
            .OrderBy(static name => name, StringComparer.Ordinal)));
    }

    public static string GetGraphFingerprint(IReadOnlyList<ModuleDependencyModel> models)
    {
        var nodes = models.Select(GetNodeDefinition).OrderBy(static node => node, StringComparer.Ordinal);
        return Hash(string.Join('\n', nodes));
    }

    private static string GetNodeDefinition(ModuleDependencyModel model)
    {
        var dependencies = model.IsDependentOn
            .Select(static dependency => ModuleTypeIdentifier.Get(dependency.Module.GetType()))
            .OrderBy(static name => name, StringComparer.Ordinal);
        return string.Join('\0',
            ModuleTypeIdentifier.GetRuntime(model.Module.GetType()),
            ModuleTypeIdentifier.GetRuntime(model.Module.ResultType),
            string.Join('\0', dependencies));
    }

    private static string Hash(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
}
