using System.Globalization;
using System.IO.Compression;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace ModularPipelines.Build.Helpers;

public static class PackedAssemblyVersionValidator
{
    public static void Validate(string packagePath)
    {
        using var package = ZipFile.OpenRead(packagePath);
        var nuspec = package.Entries.Single(entry => entry.FullName.EndsWith(".nuspec", StringComparison.OrdinalIgnoreCase));
        using var manifest = nuspec.Open();
        var metadata = XDocument.Load(manifest).Root!.Elements().Single(element => element.Name.LocalName == "metadata");
        var packageVersion = metadata.Elements().Single(element => element.Name.LocalName == "version").Value;
        var packageMajor = int.Parse(packageVersion.Split('.')[0], CultureInfo.InvariantCulture);

        // Inspect our library, tool and bundled analyzer DLLs, without loading them or their dependencies.
        // Content-only packages have no assemblies to check; third-party tool dependencies have their own versions.
        foreach (var entry in package.Entries.Where(entry =>
                     Path.GetFileName(entry.FullName).StartsWith("ModularPipelines", StringComparison.Ordinal)
                     && entry.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)))
        {
            using var stream = entry.Open();
            using var assembly = new MemoryStream();
            stream.CopyTo(assembly);
            assembly.Position = 0;
            using var pe = new PEReader(assembly);
            var assemblyVersion = pe.GetMetadataReader().GetAssemblyDefinition().Version;
            if (assemblyVersion.Major != packageMajor)
            {
                throw new InvalidOperationException(
                    $"Package '{packagePath}' version {packageVersion} contains '{entry.FullName}' with assembly version {assemblyVersion}. "
                    + "Build with the package version before packing with NoBuild.");
            }
        }
    }
}
