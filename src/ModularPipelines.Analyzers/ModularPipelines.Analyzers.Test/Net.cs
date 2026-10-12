using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Testing;

namespace ModularPipelines.Analyzers.Test;

internal static class Net
{
    private static readonly Lazy<ReferenceAssemblies> LazyNet110 = new(() =>
        new ReferenceAssemblies(
                "net11.0",
                new PackageIdentity(
                    "Microsoft.NETCore.App.Ref",
                    "11.0.0-rc.1.26425.128"),
                Path.Combine("ref", "net11.0"))
            .AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.Extensions.Logging", "11.0.0-rc.1.26425.128")))
        );

    public static ReferenceAssemblies Net110 => LazyNet110.Value;
}
