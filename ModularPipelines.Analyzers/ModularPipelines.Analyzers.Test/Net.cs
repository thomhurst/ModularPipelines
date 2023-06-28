using Microsoft.CodeAnalysis.Testing;

namespace ModularPipelines.Analyzers.Test;

public class Net
{
    private static readonly Lazy<ReferenceAssemblies> _lazyNet60 = new(() =>
        new ReferenceAssemblies(
            "net6.0",
            new PackageIdentity(
                "Microsoft.NETCore.App.Ref",
                "6.0.19"),
            Path.Combine("ref", "net6.0")));
    
    public static ReferenceAssemblies Net60 => _lazyNet60.Value;
}