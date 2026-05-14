namespace ModularPipelines.UnitTests.FSharp.Engine

open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open ModularPipelines.Context
open ModularPipelines.Extensions
open ModularPipelines.DependencyInjection
open ModularPipelines.Modules
open ModularPipelines.TestHelpers
open Moq
open TUnit.Assertions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core
open TUnit.Assertions.Extensions

type private DependencyInjectionModule() =
    inherit Module<bool>()

    override _.ExecuteAsync(_: IModuleContext, _) =
        System.Threading.Tasks.Task.FromResult(true)

type DependencyInjectionTests() =
    [<Test>]
    member _.AllDependenciesCanBeBuilt() = async {
        let! host =
            TestPipelineHostBuilder.Create()
                .AddModule<DependencyInjectionModule>()
                .BuildHostAsync()
            |> Async.AwaitTask

        let services = host.Services
        let collection = services.GetRequiredService<IPipelineServiceContainerWrapper>().ServiceCollection

        for serviceDescriptor in
            collection
            |> Seq.filter (fun sd ->
                let serviceNamespace = sd.ServiceType.Namespace
                not sd.ServiceType.IsGenericType
                && not (isNull serviceNamespace)
                && serviceNamespace.StartsWith("ModularPipeline")) do
            services.GetRequiredService(serviceDescriptor.ServiceType) |> ignore

        do! check(Assert.That(true).IsTrue())
    }

    [<Test>]
    member _.Validate() = async {
        let serviceCollection =
            ServiceCollection()
                .AddSingleton(Mock.Of<IHost>())
                .AddSingleton(Mock.Of<IHostEnvironment>())
                .AddSingleton(Mock.Of<IConfiguration>())

        DependencyInjectionSetup.Initialize(serviceCollection)

        serviceCollection.BuildServiceProvider(
            ServiceProviderOptions(
                ValidateScopes = true,
                ValidateOnBuild = true
            )
        )
        |> ignore

        do! check(Assert.That(true).IsTrue())
    }
