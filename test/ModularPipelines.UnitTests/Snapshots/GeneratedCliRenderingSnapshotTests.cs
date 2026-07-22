using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
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

    private static SortedDictionary<string, PackageSnapshot> CreateSnapshots(string repositoryRoot)
    {
        var snapshots = new SortedDictionary<string, PackageSnapshot>(StringComparer.Ordinal);

        foreach (var packageName in GetGeneratedPackageNames(repositoryRoot))
        {
            var assembly = System.Reflection.Assembly.Load(packageName);
            snapshots.Add(packageName, CreatePackageSnapshot(assembly));
        }

        return snapshots;
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
                    property.SetValue(options, CreateRepresentativeValue(property.PropertyType, isSecret));

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

    private static object CreateRepresentativeValue(Type propertyType, bool isSecret)
    {
        var type = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        if (type == typeof(string))
        {
            return isSecret ? "secret-value" : "value";
        }

        if (type == typeof(bool))
        {
            return true;
        }

        if (type == typeof(int))
        {
            return 2;
        }

        if (type == typeof(double))
        {
            return 2d;
        }

        if (type == typeof(KeyValue))
        {
            return new KeyValue("key", isSecret ? "secret-value" : "value");
        }

        if (type == typeof(CliOptionValuePair))
        {
            return new CliOptionValuePair("key", isSecret ? "secret-value" : "value");
        }

        if (type.IsEnum)
        {
            return Enum.GetValues(type).GetValue(0)
                   ?? throw new InvalidOperationException($"Generated enum {type.FullName} has no values.");
        }

        if (type.IsArray)
        {
            return CreateRepresentativeArray(type.GetElementType()!, isSecret);
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            return CreateRepresentativeArray(type.GetGenericArguments()[0], isSecret);
        }

        throw new NotSupportedException($"No representative CLI value is defined for {propertyType.FullName}.");
    }

    private static Array CreateRepresentativeArray(Type elementType, bool isSecret)
    {
        var values = Array.CreateInstance(elementType, 2);
        values.SetValue(CreateRepresentativeValue(elementType, isSecret), 0);
        values.SetValue(CreateRepresentativeValue(elementType, isSecret), 1);
        return values;
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

    private sealed record PackageSnapshot(
        int Options,
        int Properties,
        int SecretProperties,
        int RenderedArguments,
        string Sha256,
        SortedDictionary<string, string> TypeSha256);
}
