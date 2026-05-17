namespace ModularPipelines.UnitTests.FSharp.Context

open ModularPipelines.Context
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open TUnit.Core

type InterfaceVisibilityTests() =
    [<Test>]
    member _.EngineInterfaces_ShouldBeInternal() = async {
        let assembly = typeof<IModuleContext>.Assembly

        let engineInterfaces =
            assembly.GetTypes()
            |> Array.filter (fun t -> t.IsInterface)
            |> Array.filter (fun t -> t.Namespace <> null && t.Namespace.Contains("Engine"))
            |> Array.filter (fun t -> t.Name.StartsWith("IPipeline"))

        for iface in engineInterfaces do
            do! check(Assert.That(iface.IsPublic).IsFalse())
    }

    [<Test>]
    member _.UserFacingContextInterfaces_ShouldBePublic() = async {
        let assembly = typeof<IModuleContext>.Assembly

        let expectedPublicInterfaces =
            [|
                "ModularPipelines.Context", "IPipelineContext"
                "ModularPipelines.Context", "IPipelineHookContext"
                "ModularPipelines.Context", "IModuleContext"
                "ModularPipelines.Context.Domains", "IShellContext"
                "ModularPipelines.Context.Domains", "IFilesContext"
                "ModularPipelines.Context.Domains", "IDataContext"
                "ModularPipelines.Context.Domains", "IEnvironmentDomainContext"
                "ModularPipelines.Context.Domains", "IInstallersContext"
                "ModularPipelines.Context.Domains", "INetworkContext"
                "ModularPipelines.Context.Domains", "ISecurityContext"
                "ModularPipelines.Context.Domains", "IServicesContext"
            |]

        for ns, interfaceName in expectedPublicInterfaces do
            let iface = assembly.GetType($"{ns}.{interfaceName}")
            do! check(Assert.That(iface).IsNotNull())
            do! check(Assert.That(iface.IsPublic).IsTrue())
    }

    [<Test>]
    member _.ExtensionPointInterfaces_ShouldBePublic() = async {
        let assembly = typeof<IModuleContext>.Assembly

        let extensionPointInterfaces =
            [|
                "ModularPipelines", "IPipeline"
                "ModularPipelines.Interfaces", "IPipelineGlobalHooks"
                "ModularPipelines.Interfaces", "IPipelineModuleHooks"
                "ModularPipelines.Requirements", "IPipelineRequirement"
            |]

        for ns, name in extensionPointInterfaces do
            let iface = assembly.GetType($"{ns}.{name}")
            do! check(Assert.That(iface).IsNotNull())
            do! check(Assert.That(iface.IsPublic).IsTrue())
    }

    [<Test>]
    member _.IPipelineServiceContainerWrapper_ShouldBeInternal() = async {
        let assembly = typeof<IModuleContext>.Assembly
        let iface = assembly.GetType("ModularPipelines.DependencyInjection.IPipelineServiceContainerWrapper")

        do! check(Assert.That(iface).IsNotNull())
        do! check(Assert.That(not iface.IsPublic).IsTrue())
    }
