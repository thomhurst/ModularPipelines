using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Context.Linux;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines;

internal static class DependencyInjectionSetup
{
    public static void Initialize(IServiceCollection services)
    {
        // Bundles
        services
            .Configure<PipelineOptions>(_ => { })
            .AddLogging()
            .AddHttpClient()
            .AddInitializers()
            .AddServiceCollection();

        // Transient
        services.AddScoped<IModuleContext, ModuleContext>()
            .AddSingleton<IModuleLoggerProvider, ModuleLoggerProvider>()
            .AddScoped<IHttp, Http>()
            .AddScoped<ICommand, Command>()
            .AddScoped<ICertificates, Certificates>()
            .AddScoped<IDownloader, Downloader>()
            .AddScoped<IInstaller, Installer>()
            .AddScoped<IFileInstaller, FileInstaller>()
            .AddScoped<IPredefinedInstallers, PredefinedInstallers>()
            .AddScoped<IWindowsInstaller, WindowsInstaller>()
            .AddScoped<IMacInstaller, MacInstaller>()
            .AddScoped<ILinuxInstaller, LinuxInstaller>()
            .AddScoped<IAptGet, AptGet>()
            .AddScoped<IHasher, Hasher>()
            .AddScoped<IBase64, Base64>()
            .AddScoped<IHex, Hex>()
            .AddScoped<IZip, Zip>()
            .AddScoped<IJson, Json>()
            .AddScoped<IXml, Xml>()
            .AddScoped<IPowershell, Powershell>()
            .AddScoped<IBash, Bash>()
            .AddScoped<ISecretObfuscator, SecretObfuscator>()
            .AddScoped<IOptionsProvider, OptionsProvider>()
            .AddScoped<IBuildSystemDetector, BuildSystemDetector>()
            .AddScoped(typeof(ModuleLogger<>))
            .AddScoped<IEnvironmentContext, EnvironmentContext>()
            .AddScoped<IEnvironmentVariables, EnvironmentVariables>()
            .AddScoped<IFileSystemContext, FileSystemContext>()
            .AddScoped<ISafeModuleEstimatedTimeProvider, SafeModuleEstimatedTimeProvider>();

        // Singletons
        services
            .AddSingleton<IAsyncLocalModule, AsyncLocalModule>()
            .AddSingleton<IModuleStatusProvider, ModuleStatusProvider>()
            .AddSingleton<EngineCancellationToken>()
            .AddSingleton<IPipelineInitializer, PipelineInitializer>()
            .AddSingleton<IPipelineSetupExecutor, PipelineSetupExecutor>()
            .AddSingleton<IModuleIgnoreHandler, ModuleIgnoreHandler>()
            .AddSingleton<IPipelineConsolePrinter, PipelineConsoleProgressPrinter>()
            .AddSingleton<IPipelineExecutor, PipelineExecutor>()
            .AddSingleton<IModuleExecutor, ModuleExecutor>()
            .AddSingleton<IModuleLoggerContainer, ModuleLoggerContainer>()
            .AddSingleton<IAssemblyLoadedTypesProvider, AssemblyLoadedTypesProvider>()
            .AddSingleton<IDependencyChainProvider, DependencyChainProvider>()
            .AddSingleton<IDependencyDetector, DependencyDetector>()
            .AddSingleton<IUnusedModuleDetector, UnusedModuleDetector>()
            .AddSingleton<IDependencyCollisionDetector, DependencyCollisionDetector>()
            .AddSingleton<IDependencyPrinter, DependencyPrinter>()
            .AddSingleton<IRequirementChecker, RequirementChecker>()
            .AddSingleton<IModuleRetriever, ModuleRetriever>()
            .AddSingleton<IModuleResultRepository, NoOpModuleResultRepository>()
            .AddSingleton<IModuleEstimatedTimeProvider, FileSystemModuleEstimatedTimeProvider>();
    }
}