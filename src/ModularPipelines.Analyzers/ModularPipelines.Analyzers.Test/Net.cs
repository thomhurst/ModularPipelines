using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Testing;

namespace ModularPipelines.Analyzers.Test;

internal static class Net
{
    private static readonly Lazy<ReferenceAssemblies> LazyNet60 = new(() =>
        new ReferenceAssemblies(
                "net6.0",
                new PackageIdentity(
                    "Microsoft.NETCore.App.Ref",
                    "6.0.21"),
                Path.Combine("ref", "net6.0"))
            .AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.Extensions.Logging", "6.0.0")))
        );

    public static ReferenceAssemblies Net60 => LazyNet60.Value;

    private static readonly Lazy<ReferenceAssemblies> LazyNet80 = new(() =>
        new ReferenceAssemblies(
                "net8.0",
                new PackageIdentity(
                    "Microsoft.NETCore.App.Ref",
                    "8.0.6"),
                Path.Combine("ref", "net8.0"))
            .AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.Extensions.Logging", "8.0.0")))
    );

    public static ReferenceAssemblies Net80 => LazyNet80.Value;
}