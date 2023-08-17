using Microsoft.CodeAnalysis.Testing;

namespace ModularPipelines.Analyzers.Test;

internal class Net
{
    private static readonly Lazy<ReferenceAssemblies> LazyNet60 = new(() =>
        new ReferenceAssemblies(
            "net6.0",
            new PackageIdentity(
                "Microsoft.NETCore.App.Ref",
                "6.0.21"),
            Path.Combine("ref", "net6.0")));

    public static ReferenceAssemblies Net60 => LazyNet60.Value;
    
    private static readonly Lazy<ReferenceAssemblies> LazyNet70 = new(() =>
        new ReferenceAssemblies(
            "net7.0",
            new PackageIdentity(
                "Microsoft.NETCore.App.Ref",
                "7.0.10"),
            Path.Combine("ref", "net7.0")));

    public static ReferenceAssemblies Net70 => LazyNet70.Value;
}
