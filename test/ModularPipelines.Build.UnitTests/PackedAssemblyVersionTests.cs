using System.IO.Compression;
using ModularPipelines.Build.Helpers;

namespace ModularPipelines.Build.UnitTests;

public class PackedAssemblyVersionTests
{
    [Test]
    [Arguments("lib/net11.0/ModularPipelines.dll")]
    [Arguments("lib/netstandard2.0/ModularPipelines.dll")]
    [Arguments("analyzers/dotnet/cs/ModularPipelines.SourceGenerator.dll")]
    [Arguments("tools/net11.0/any/ModularPipelines.OptionsGenerator.dll")]
    public void Rejects_Assembly_Major_That_Differs_From_Package(string entry)
    {
        var major = typeof(PackedAssemblyVersionTests).Assembly.GetName().Version!.Major;
        WithPackage($"{major + 1}.0.0-alpha0001", [entry], package =>
        {
            var exception = Assert.Throws<InvalidOperationException>(() => PackedAssemblyVersionValidator.Validate(package));
            if (!exception.Message.Contains(entry, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("The error must identify the mismatched DLL.");
            }
        });
    }

    [Test]
    [Arguments("")]
    [Arguments("-alpha0001")]
    [Arguments("-beta")]
    public void Accepts_Matching_Assemblies_In_Every_Target_And_Bundled_Analyzers(string suffix)
    {
        var major = typeof(PackedAssemblyVersionTests).Assembly.GetName().Version!.Major;
        WithPackage($"{major}.2.3{suffix}",
            ["lib/net11.0/ModularPipelines.dll", "lib/netstandard2.0/ModularPipelines.dll", "analyzers/dotnet/cs/ModularPipelines.SourceGenerator.dll"],
            PackedAssemblyVersionValidator.Validate);
    }

    [Test]
    public void Checks_Later_Assemblies_Even_When_An_Earlier_One_Matches()
    {
        var major = typeof(PackedAssemblyVersionTests).Assembly.GetName().Version!.Major;
        var otherAssembly = typeof(object).Assembly;
        var otherMajor = otherAssembly.GetName().Version!.Major;
        if (major == otherMajor)
        {
            throw new InvalidOperationException("This fixture requires distinct assembly majors.");
        }

        WithPackage($"{major}.0.0", ["lib/net11.0/ModularPipelines.dll"], package =>
        {
            using (var archive = ZipFile.Open(package, ZipArchiveMode.Update))
            {
                using var stream = archive.CreateEntry("analyzers/dotnet/cs/ModularPipelines.SourceGenerator.dll").Open();
                using var source = File.OpenRead(otherAssembly.Location);
                source.CopyTo(stream);
            }

            Assert.Throws<InvalidOperationException>(() => PackedAssemblyVersionValidator.Validate(package));
        });
    }

    [Test]
    public void Accepts_Content_Only_Packages_And_Independent_Third_Party_Dependencies()
    {
        WithPackage("4.0.0", ["tools/net11.0/any/ThirdParty.dll"], PackedAssemblyVersionValidator.Validate);
        WithPackage("4.0.0", [], PackedAssemblyVersionValidator.Validate);
    }

    private static void WithPackage(string version, string[] assemblies, Action<string> action)
    {
        var path = Path.Combine(Path.GetTempPath(), $"packed-version-{Guid.NewGuid():N}.nupkg");
        try
        {
            using (var archive = ZipFile.Open(path, ZipArchiveMode.Create))
            {
                using (var writer = new StreamWriter(archive.CreateEntry("ModularPipelines.nuspec").Open()))
                {
                    writer.Write($"<package xmlns=\"http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd\"><metadata><id>ModularPipelines</id><version>{version}</version></metadata></package>");
                }

                foreach (var assembly in assemblies)
                {
                    using var stream = archive.CreateEntry(assembly).Open();
                    using var source = File.OpenRead(typeof(PackedAssemblyVersionTests).Assembly.Location);
                    source.CopyTo(stream);
                }
            }

            action(path);
        }
        finally
        {
            File.Delete(path);
        }
    }
}
