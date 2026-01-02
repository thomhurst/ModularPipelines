using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Testing;

namespace ModularPipelines.Development.Analyzers.UnitTests;

internal static class Net
{
    private static readonly Lazy<ReferenceAssemblies> LazyNet80 = new(() =>
        new ReferenceAssemblies(
                "net8.0",
                new PackageIdentity(
                    "Microsoft.NETCore.App.Ref",
                    "8.0.6"),
                Path.Combine("ref", "net8.0"))
            .AddPackages(ImmutableArray.Create(new PackageIdentity("System.Threading.Tasks", "4.3.0")))
    );

    public static ReferenceAssemblies Net80 => LazyNet80.Value;
}
