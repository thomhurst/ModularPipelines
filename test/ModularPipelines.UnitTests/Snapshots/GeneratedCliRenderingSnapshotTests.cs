using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Snapshots;

public class GeneratedCliRenderingSnapshotTests
{
    private const string SnapshotFileName = "GeneratedCliRendering.snap.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
    };

    [Test]
    public async Task EveryGeneratedOptionsClass_MatchesRenderingSnapshot()
    {
        var repositoryRoot = FindRepositoryRoot();
        var actual = CreateSnapshots(repositoryRoot);
        var actualJson = Serialize(actual);
        var snapshotPath = Path.Combine(repositoryRoot, "test", "ModularPipelines.UnitTests", "Snapshots", SnapshotFileName);

        if (string.Equals(
                Environment.GetEnvironmentVariable("UPDATE_CLI_RENDERING_SNAPSHOTS"),
                "true",
                StringComparison.OrdinalIgnoreCase))
        {
            await File.WriteAllTextAsync(snapshotPath, actualJson);
            return;
        }

        var expectedJson = (await File.ReadAllTextAsync(snapshotPath)).ReplaceLineEndings("\n");
        if (!string.Equals(expectedJson, actualJson, StringComparison.Ordinal))
        {
            await File.WriteAllTextAsync(snapshotPath + ".received.json", actualJson);
        }

        await Assert.That(actualJson).IsEqualTo(expectedJson)
            .Because($"Generated CLI rendering changed. Inspect {SnapshotFileName}.received.json and update the snapshot intentionally.");
    }

    [Test]
    public async Task CommandSerialization_PreservesArgumentBoundaries()
    {
        var separateArguments = SerializeCommandLine(new CommandLine("tool", ["--opt", "value"]));
        var combinedArgument = SerializeCommandLine(new CommandLine("tool", ["--opt value"]));

        await Assert.That(separateArguments).IsNotEqualTo(combinedArgument);
    }

    [Test]
    public async Task GeneratedOptionsDetection_IncludesCheckedInGeneratedOptions()
    {
        var optionsType = typeof(DotNet.Generated.Options.DotNetPackOptions);

        await Assert.That(optionsType.GetCustomAttribute<GeneratedCodeAttribute>()).IsNull();
        await Assert.That(IsGeneratedOptionsType(optionsType)).IsTrue();
    }

    [Test]
    public async Task Representative_Array_Values_Are_Distinct()
    {
        var values = (string[]) CreateRepresentativeValue(typeof(string[]), isSecret: false, "Values");

        await Assert.That(values[0]).IsNotEqualTo(values[1]);
    }

    [Test]
    public async Task Representative_Property_Values_Are_Distinct()
    {
        var first = CreateRepresentativeValue(typeof(string), isSecret: false, "First");
        var second = CreateRepresentativeValue(typeof(string), isSecret: false, "Second");

        await Assert.That(first).IsNotEqualTo(second);
    }

    [Test]
    public async Task GeneratedPackageDiscovery_OnlyIncludesAvailableAssemblies()
    {
        var packageNames = GetGeneratedPackageNames(FindRepositoryRoot());

        foreach (var packageName in packageNames)
        {
            await Assert.That(File.Exists(Path.Combine(AppContext.BaseDirectory, $"{packageName}.dll"))).IsTrue();
        }
    }

    private static SortedDictionary<string, PackageSnapshot> CreateSnapshots(string repositoryRoot)
    {
        var snapshots = new SortedDictionary<string, PackageSnapshot>(StringComparer.Ordinal);

        foreach (var packageName in GetGeneratedPackageNames(repositoryRoot))
        {
            var package = LoadGeneratedPackageAssembly(repositoryRoot, packageName);
            try
            {
                snapshots.Add(packageName, CreatePackageSnapshot(package.Assembly));
            }
            finally
            {
                package.LoadContext.Unload();
            }
        }

        return snapshots;
    }

    private static LoadedPackage LoadGeneratedPackageAssembly(
        string repositoryRoot,
        string packageName)
    {
        var configuration = new DirectoryInfo(AppContext.BaseDirectory).Parent?.Name
                            ?? throw new DirectoryNotFoundException("Could not determine the build configuration.");
        var assemblyPath = Path.Combine(AppContext.BaseDirectory, $"{packageName}.dll");
        if (!File.Exists(assemblyPath))
        {
            throw new FileNotFoundException($"Could not load built package {packageName}.", assemblyPath);
        }

        var loadContext = new PackageAssemblyLoadContext(repositoryRoot, configuration, assemblyPath);
        return new LoadedPackage(loadContext.LoadFromAssemblyPath(Path.GetFullPath(assemblyPath)), loadContext);
    }

    private static PackageSnapshot CreatePackageSnapshot(System.Reflection.Assembly assembly)
    {
        var modelProvider = new CommandModelProvider();
        var commandBuilder = new CommandLineBuilder(
            new ToolResolver(),
            new CommandPartsProvider(),
            new PlaceholderHandler(modelProvider),
            modelProvider,
            new CommandArgumentBuilder());
        var corpus = new StringBuilder();
        var propertyCount = 0;
        var secretPropertyCount = 0;
        var renderedArgumentCount = 0;
        var typeHashes = new SortedDictionary<string, string>(StringComparer.Ordinal);
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var optionTypes = assembly.GetTypes()
                .Where(IsGeneratedOptionsType)
                .OrderBy(x => x.FullName, StringComparer.Ordinal)
                .ToArray();

            foreach (var optionType in optionTypes)
            {
                var options = (CommandLineToolOptions) RuntimeHelpers.GetUninitializedObject(optionType);
                var commandModel = modelProvider.GetCommandModel(optionType);
                var properties = commandModel.ToDictionary(
                    part => part.PropertyName,
                    part => GetProperty(optionType, part.PropertyName),
                    StringComparer.Ordinal);
                var secretPropertyNames = GetSecretPropertyNames(optionType);
                var typeCorpus = new StringBuilder();

                foreach (var part in commandModel)
                {
                    var property = properties[part.PropertyName];
                    var isSecret = secretPropertyNames.Contains(part.PropertyName);
                    property.SetValue(options, CreateRepresentativeValue(property.PropertyType, isSecret, part.PropertyName));

                    propertyCount++;
                    secretPropertyCount += isSecret ? 1 : 0;
                }

                var commandLine = commandBuilder.Build(options);
                renderedArgumentCount += commandLine.Arguments.Count;

                typeCorpus.Append(optionType.FullName).Append('|');
                foreach (var part in commandModel)
                {
                    var property = properties[part.PropertyName];
                    typeCorpus.Append(part.GetType().Name)
                        .Append(':')
                        .Append(part.PropertyName)
                        .Append(':')
                        .Append(property.PropertyType.FullName)
                        .Append(':')
                        .Append(secretPropertyNames.Contains(part.PropertyName) ? "secret" : "plain")
                        .Append(';');
                }

                typeCorpus.Append('|').Append(SerializeCommandLine(commandLine)).Append('\n');
                var serializedType = typeCorpus.ToString();
                corpus.Append(serializedType);
                typeHashes.Add(optionType.FullName!, Hash(serializedType));
            }

            return new PackageSnapshot(
                optionTypes.Length,
                propertyCount,
                secretPropertyCount,
                renderedArgumentCount,
                Hash(corpus.ToString()),
                typeHashes);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    private static bool IsGeneratedOptionsType(Type type)
    {
        return !type.IsAbstract
               && type.IsAssignableTo(typeof(CommandLineToolOptions))
               && (type.GetCustomAttribute<GeneratedCodeAttribute>()?.Tool == "ModularPipelines.OptionsGenerator"
                   || type.Namespace?.EndsWith(".Generated.Options", StringComparison.Ordinal) == true);
    }

    private static PropertyInfo GetProperty(Type optionsType, string propertyName)
    {
        for (var current = optionsType; current is not null; current = current.BaseType)
        {
            var property = current.GetProperty(
                propertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            if (property is not null)
            {
                return property;
            }
        }

        throw new InvalidOperationException($"Could not find property {propertyName} on {optionsType.FullName}.");
    }

    private static HashSet<string> GetSecretPropertyNames(Type optionsType)
    {
        return GeneratedSecretMetadata.TryGetAccessors(optionsType, out var accessors)
            ? accessors.Select(accessor => accessor.PropertyName).ToHashSet(StringComparer.Ordinal)
            : [];
    }

    private static string Hash(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();

    private static object CreateRepresentativeValue(
        Type propertyType,
        bool isSecret,
        string discriminator,
        int occurrence = 0)
    {
        var type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
        var suffix = $"{discriminator}-{occurrence + 1}";
        var representativeValue = $"{(isSecret ? "secret" : "value")}-{suffix}";

        if (type == typeof(string))
        {
            return representativeValue;
        }

        if (type == typeof(bool))
        {
            return occurrence % 2 == 0;
        }

        if (type == typeof(int))
        {
            return GetRepresentativeNumber(discriminator, occurrence);
        }

        if (type == typeof(double))
        {
            return (double) GetRepresentativeNumber(discriminator, occurrence);
        }

        if (type == typeof(KeyValue))
        {
            return new KeyValue($"key-{suffix}", representativeValue);
        }

        if (type == typeof(CliOptionValuePair))
        {
            return new CliOptionValuePair($"key-{suffix}", representativeValue);
        }

        if (type.IsEnum)
        {
            var values = Enum.GetValues(type);
            if (values.Length == 0)
            {
                throw new InvalidOperationException($"Generated enum {type.FullName} has no values.");
            }

            return values.GetValue(GetRepresentativeNumber(discriminator, occurrence) % values.Length)!;
        }

        if (type.IsArray)
        {
            return CreateRepresentativeArray(type.GetElementType()!, isSecret, discriminator);
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            return CreateRepresentativeArray(type.GetGenericArguments()[0], isSecret, discriminator);
        }

        throw new NotSupportedException($"No representative CLI value is defined for {propertyType.FullName}.");
    }

    private static Array CreateRepresentativeArray(Type elementType, bool isSecret, string discriminator)
    {
        var values = Array.CreateInstance(elementType, 2);
        values.SetValue(CreateRepresentativeValue(elementType, isSecret, discriminator), 0);
        values.SetValue(CreateRepresentativeValue(elementType, isSecret, discriminator, occurrence: 1), 1);
        return values;
    }

    private static int GetRepresentativeNumber(string discriminator, int occurrence)
    {
        unchecked
        {
            var hash = 17;
            foreach (var character in discriminator)
            {
                hash = (hash * 31) + character;
            }

            return Math.Abs(hash % 10_000) + occurrence + 2;
        }
    }

    private static IEnumerable<string> GetGeneratedPackageNames(string repositoryRoot)
    {
        var sourceDirectory = Path.Combine(repositoryRoot, "src");

        return Directory.EnumerateDirectories(sourceDirectory, "ModularPipelines.*")
            .Where(packageDirectory => Directory.Exists(Path.Combine(packageDirectory, "Options")))
            .Where(packageDirectory => Directory.EnumerateFiles(
                Path.Combine(packageDirectory, "Options"),
                "*.Generated.cs",
                SearchOption.TopDirectoryOnly).Any())
            .Select(Path.GetFileName)
            .OfType<string>()
            .OrderBy(x => x, StringComparer.Ordinal);
    }

    private static string FindRepositoryRoot()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, "src"))
                && File.Exists(Path.Combine(directory.FullName, "global.json")))
            {
                return directory.FullName;
            }
        }

        throw new DirectoryNotFoundException("Could not locate the ModularPipelines repository root.");
    }

    private static string Serialize(SortedDictionary<string, PackageSnapshot> snapshots)
    {
        return JsonSerializer.Serialize(snapshots, JsonOptions).ReplaceLineEndings("\n") + "\n";
    }

    private static string SerializeCommandLine(CommandLine commandLine) =>
        JsonSerializer.Serialize(commandLine, JsonOptions);

    private sealed record LoadedPackage(
        System.Reflection.Assembly Assembly,
        PackageAssemblyLoadContext LoadContext);

    private sealed class PackageAssemblyLoadContext(string repositoryRoot, string configuration, string assemblyPath) : AssemblyLoadContext(isCollectible: true)
    {
        private static readonly System.Reflection.Assembly[] SharedAssemblies =
        [
            typeof(CommandLineToolOptions).Assembly,
            typeof(Microsoft.Extensions.DependencyInjection.IServiceCollection).Assembly,
        ];

        private readonly string _repositoryRoot = repositoryRoot;
        private readonly string _configuration = configuration;
        private readonly AssemblyDependencyResolver _resolver = new(assemblyPath);
        private readonly Dictionary<string, string> _packageDependencyPaths = GetPackageDependencyPaths(assemblyPath);

        protected override System.Reflection.Assembly? Load(AssemblyName assemblyName)
        {
            var sharedAssembly = SharedAssemblies.FirstOrDefault(assembly =>
                string.Equals(assembly.GetName().Name, assemblyName.Name, StringComparison.Ordinal));
            if (sharedAssembly is not null)
            {
                return sharedAssembly;
            }

            var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName)
                               ?? GetProjectAssemblyPath(assemblyName.Name!)
                               ?? _packageDependencyPaths.GetValueOrDefault(assemblyName.Name!);
            return assemblyPath is null ? null : LoadFromAssemblyPath(assemblyPath);
        }

        private string? GetProjectAssemblyPath(string assemblyName)
        {
            var outputDirectory = Path.Combine(_repositoryRoot, "src", assemblyName, "bin", _configuration);
            return Directory.Exists(outputDirectory)
                ? Directory.EnumerateFiles(outputDirectory, $"{assemblyName}.dll", SearchOption.AllDirectories)
                    .FirstOrDefault()
                : null;
        }

        private static Dictionary<string, string> GetPackageDependencyPaths(string assemblyPath)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var dependencyManifestPath = Path.ChangeExtension(assemblyPath, ".deps.json");
            if (!File.Exists(dependencyManifestPath))
            {
                return result;
            }

            using var manifest = JsonDocument.Parse(File.ReadAllText(dependencyManifestPath));
            var root = manifest.RootElement;
            var targetName = root.GetProperty("runtimeTarget").GetProperty("name").GetString()!;
            var libraries = root.GetProperty("libraries");
            var packagesDirectory = Environment.GetEnvironmentVariable("NUGET_PACKAGES")
                                    ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");

            foreach (var library in root.GetProperty("targets").GetProperty(targetName).EnumerateObject())
            {
                if (!library.Value.TryGetProperty("runtime", out var runtimeAssets)
                    || !libraries.GetProperty(library.Name).TryGetProperty("path", out var packagePath))
                {
                    continue;
                }

                foreach (var runtimeAsset in runtimeAssets.EnumerateObject())
                {
                    if (!runtimeAsset.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var dependencyPath = Path.Combine(
                        packagesDirectory,
                        packagePath.GetString()!.Replace('/', Path.DirectorySeparatorChar),
                        runtimeAsset.Name.Replace('/', Path.DirectorySeparatorChar));
                    result[Path.GetFileNameWithoutExtension(runtimeAsset.Name)] = dependencyPath;
                }
            }

            return result;
        }
    }

    private sealed record PackageSnapshot(
        int Options,
        int Properties,
        int SecretProperties,
        int RenderedArguments,
        string Sha256,
        SortedDictionary<string, string> TypeSha256);
}
