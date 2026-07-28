using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Testing;

namespace ModularPipelines.Analyzers.Test;

internal static class Net
{
    private static readonly Lazy<ReferenceAssemblies> LazyNet100 = new(() =>
        new ReferenceAssemblies(
                "net10.0",
                new PackageIdentity(
                    "Microsoft.NETCore.App.Ref",
                    "10.0.10"),
                Path.Combine("ref", "net10.0"))
            .AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.Extensions.Logging", "10.0.10")))
        );

    public static ReferenceAssemblies Net100 => LazyNet100.Value;
}
