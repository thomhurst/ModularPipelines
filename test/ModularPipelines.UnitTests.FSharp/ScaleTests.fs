namespace ModularPipelines.UnitTests.FSharp

open ModularPipelines.TestHelpers
open System.Collections.Concurrent
open System.Reflection
open System.Threading
open ModularPipelines.Modules
open ModularPipelines.Context
open TUnit.Core
open TUnit.Assertions
open TUnit.Assertions.Extensions
open TUnit.Assertions.FSharp.Operations
open ModularPipelines.Extensions
open Microsoft.Extensions.DependencyInjection
open ModularPipelines.Enums

module ScaleTestsModule =
    type ExecutionRecord = {
        ModuleName: string
        CompletionOrder : int
        StartTime: System.DateTimeOffset
        EndTime: System.DateTimeOffset
    }
    /// <summary>
    /// Thread-safe tracker for recording module execution order and timing. Each test creates a fresh instance for test
    /// isolation.
    /// </summary>
    type ExecutionTracker() = 
        let _executionRecords = ConcurrentDictionary<string, ExecutionRecord>()
        let mutable _completionCounter = 0

        /// <summary>
        /// Records the start of module execution.
        /// </summary>
        /// <param name="moduleName">The name/type of the module.</param>
        member _.RecordStart(moduleName: string) =
            let record = { ModuleName = moduleName; CompletionOrder = 0; StartTime = System.DateTimeOffset.UtcNow; EndTime = System.DateTimeOffset.MinValue }
            _executionRecords[moduleName] <- record
        /// <summary>
        /// Marks a module as completed and records completion order.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        member _.MarkCompleted(moduleName: string) =
            let completionOrder = Interlocked.Increment(ref _completionCounter);
            match _executionRecords.TryGetValue(moduleName) with
            | true, record ->
                let updatedRecord = { record with CompletionOrder = completionOrder; EndTime = System.DateTimeOffset.UtcNow }
                _executionRecords[moduleName] <- updatedRecord
            | false, _ -> ()

        /// <summary>
        /// Checks if a module has completed.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>True if the module has completed.</returns>
        member _.IsCompleted(moduleName: string) =
            match _executionRecords.TryGetValue(moduleName) with
            | true, record -> record.EndTime <> System.DateTimeOffset.MinValue
            | false, _ -> false

        /// <summary>
        /// Gets all execution records ordered by completion order.
        /// </summary>
        member _.GetRecords() =
            _executionRecords.Values
            |> Seq.sortBy (fun r -> r.CompletionOrder)
            |> Seq.toList
        
        /// <summary>
        /// Gets a specific execution record by module name.
        /// </summary>
        member _.GetRecord(moduleName: string) =
            match _executionRecords.TryGetValue(moduleName) with
            | true, record -> Some record
            | false, _ -> None

        /// <summary>
        /// Gets the count of completed modules.
        /// </summary>
        member _.CompletedCount() =
            _executionRecords.Values |> Seq.sumBy (fun r -> if r.CompletionOrder > 0 then 1 else 0)

        /// <summary>
        /// Gets the total count of recorded modules (started or completed).
        /// </summary>
        member _.TotalCount() = _executionRecords.Count

         /// <summary>
         /// Gets whether the tracker is in a clean state (no records).
         /// </summary>
        member _.IsClean() =
            _executionRecords.IsEmpty && _completionCounter = 0
    
    [<Struct>] type M1 = struct end
    [<Struct>] type M2 = struct end
    [<Struct>] type M3 = struct end
    [<Struct>] type M4 = struct end
    [<Struct>] type M5 = struct end
    [<Struct>] type M6 = struct end
    [<Struct>] type M7 = struct end
    [<Struct>] type M8 = struct end
    [<Struct>] type M9 = struct end
    [<Struct>] type M10 = struct end
    [<Struct>] type M11 = struct end
    [<Struct>] type M12 = struct end
    [<Struct>] type M13 = struct end
    [<Struct>] type M14 = struct end
    [<Struct>] type M15 = struct end
    [<Struct>] type M16 = struct end
    [<Struct>] type M17 = struct end
    [<Struct>] type M18 = struct end
    [<Struct>] type M19 = struct end
    [<Struct>] type M20 = struct end
    [<Struct>] type M21 = struct end
    [<Struct>] type M22 = struct end
    [<Struct>] type M23 = struct end
    [<Struct>] type M24 = struct end
    [<Struct>] type M25 = struct end
    [<Struct>] type M26 = struct end
    [<Struct>] type M27 = struct end
    [<Struct>] type M28 = struct end
    [<Struct>] type M29 = struct end
    [<Struct>] type M30 = struct end
    [<Struct>] type M31 = struct end
    [<Struct>] type M32 = struct end
    [<Struct>] type M33 = struct end
    [<Struct>] type M34 = struct end
    [<Struct>] type M35 = struct end
    [<Struct>] type M36 = struct end
    [<Struct>] type M37 = struct end
    [<Struct>] type M38 = struct end
    [<Struct>] type M39 = struct end
    [<Struct>] type M40 = struct end
    [<Struct>] type M41 = struct end
    [<Struct>] type M42 = struct end
    [<Struct>] type M43 = struct end
    [<Struct>] type M44 = struct end
    [<Struct>] type M45 = struct end
    [<Struct>] type M46 = struct end
    [<Struct>] type M47 = struct end
    [<Struct>] type M48 = struct end
    [<Struct>] type M49 = struct end
    [<Struct>] type M50 = struct end
    [<Struct>] type M51 = struct end
    [<Struct>] type M52 = struct end
    [<Struct>] type M53 = struct end
    [<Struct>] type M54 = struct end
    [<Struct>] type M55 = struct end
    [<Struct>] type M56 = struct end
    [<Struct>] type M57 = struct end
    [<Struct>] type M58 = struct end
    [<Struct>] type M59 = struct end
    [<Struct>] type M60 = struct end
    [<Struct>] type M61 = struct end
    [<Struct>] type M62 = struct end
    [<Struct>] type M63 = struct end
    [<Struct>] type M64 = struct end
    [<Struct>] type M65 = struct end
    [<Struct>] type M66 = struct end
    [<Struct>] type M67 = struct end
    [<Struct>] type M68 = struct end
    [<Struct>] type M69 = struct end
    [<Struct>] type M70 = struct end
    [<Struct>] type M71 = struct end
    [<Struct>] type M72 = struct end
    [<Struct>] type M73 = struct end
    [<Struct>] type M74 = struct end
    [<Struct>] type M75 = struct end
    [<Struct>] type M76 = struct end
    [<Struct>] type M77 = struct end
    [<Struct>] type M78 = struct end
    [<Struct>] type M79 = struct end
    [<Struct>] type M80 = struct end
    [<Struct>] type M81 = struct end
    [<Struct>] type M82 = struct end
    [<Struct>] type M83 = struct end
    [<Struct>] type M84 = struct end
    [<Struct>] type M85 = struct end
    [<Struct>] type M86 = struct end
    [<Struct>] type M87 = struct end
    [<Struct>] type M88 = struct end
    [<Struct>] type M89 = struct end
    [<Struct>] type M90 = struct end
    [<Struct>] type M91 = struct end
    [<Struct>] type M92 = struct end
    [<Struct>] type M93 = struct end
    [<Struct>] type M94 = struct end
    [<Struct>] type M95 = struct end
    [<Struct>] type M96 = struct end
    [<Struct>] type M97 = struct end
    [<Struct>] type M98 = struct end
    [<Struct>] type M99 = struct end
    [<Struct>] type M100 = struct end

    /// <summary>
    /// A generic module that can be instantiated with different type markers to create unique types.
    /// </summary>
    /// <typeparam name="T">A marker type that makes this module unique.</typeparam>
    type ScaleModule<'T>(tracker: ExecutionTracker) =
        inherit Module<string>()
        override _.ExecuteAsync(context: IModuleContext, cancellationToken: System.Threading.CancellationToken) =
            task {
                let typeName = typeof<'T>.Name
                tracker.RecordStart(typeName);
                tracker.MarkCompleted(typeName);
                return typeName;
            }

    /// <summary>
    /// Tests for large-scale pipeline scenarios to validate scalability, performance, and correct behavior with many
    /// modules.
    /// </summary>
    /// <remarks>
    /// These tests verify that the pipeline engine handles large numbers of modules correctly, including proper
    /// parallelization, dependency ordering, and resource management. Note: ModularPipelines requires each module to be
    /// a unique type (by design, dependencies are resolved by type). These tests use generic types with different type
    /// parameters to create many unique module types at compile time.
    /// </remarks>
    [<NotInParallel>]
    type ScaleTests() =
        inherit TestBase()

        /// <summary>
        /// Verifies that a pipeline with 100 independent modules completes successfully and all modules execute.
        /// </summary>
        /// <remarks>
        /// This test validates: - The pipeline can handle 100 modules without issues - All modules complete
        /// successfully - The pipeline status is successful
        /// </remarks>
        [<Test>]
        member _.Pipeline_With100IndependentModules_CompletesSuccessfully() =
            async {
                let tracker = new ExecutionTracker();
                do! check(Assert.That<bool>(tracker.IsClean).IsTrue())
                let expectedModuleCount = 100
                let builder =
                    TestPipelineHostBuilder.Create()
                        .ConfigureServices(fun _ services -> services.AddSingleton(tracker) |> ignore)
                        .AddModule<ScaleModule<M1>>().AddModule<ScaleModule<M2>>().AddModule<ScaleModule<M3>>()
                        .AddModule<ScaleModule<M4>>().AddModule<ScaleModule<M5>>().AddModule<ScaleModule<M6>>()
                        .AddModule<ScaleModule<M7>>().AddModule<ScaleModule<M8>>().AddModule<ScaleModule<M9>>()
                        .AddModule<ScaleModule<M10>>().AddModule<ScaleModule<M11>>().AddModule<ScaleModule<M12>>()
                        .AddModule<ScaleModule<M13>>().AddModule<ScaleModule<M14>>().AddModule<ScaleModule<M15>>()
                        .AddModule<ScaleModule<M16>>().AddModule<ScaleModule<M17>>().AddModule<ScaleModule<M18>>()
                        .AddModule<ScaleModule<M19>>().AddModule<ScaleModule<M20>>().AddModule<ScaleModule<M21>>()
                        .AddModule<ScaleModule<M22>>().AddModule<ScaleModule<M23>>().AddModule<ScaleModule<M24>>()
                        .AddModule<ScaleModule<M25>>().AddModule<ScaleModule<M26>>().AddModule<ScaleModule<M27>>()
                        .AddModule<ScaleModule<M28>>().AddModule<ScaleModule<M29>>().AddModule<ScaleModule<M30>>()
                        .AddModule<ScaleModule<M31>>().AddModule<ScaleModule<M32>>().AddModule<ScaleModule<M33>>()
                        .AddModule<ScaleModule<M34>>().AddModule<ScaleModule<M35>>().AddModule<ScaleModule<M36>>()
                        .AddModule<ScaleModule<M37>>().AddModule<ScaleModule<M38>>().AddModule<ScaleModule<M39>>()
                        .AddModule<ScaleModule<M40>>().AddModule<ScaleModule<M41>>().AddModule<ScaleModule<M42>>()
                        .AddModule<ScaleModule<M43>>().AddModule<ScaleModule<M44>>().AddModule<ScaleModule<M45>>()
                        .AddModule<ScaleModule<M46>>().AddModule<ScaleModule<M47>>().AddModule<ScaleModule<M48>>()
                        .AddModule<ScaleModule<M49>>().AddModule<ScaleModule<M50>>().AddModule<ScaleModule<M51>>()
                        .AddModule<ScaleModule<M52>>().AddModule<ScaleModule<M53>>().AddModule<ScaleModule<M54>>()
                        .AddModule<ScaleModule<M55>>().AddModule<ScaleModule<M56>>().AddModule<ScaleModule<M57>>()
                        .AddModule<ScaleModule<M58>>().AddModule<ScaleModule<M59>>().AddModule<ScaleModule<M60>>()
                        .AddModule<ScaleModule<M61>>().AddModule<ScaleModule<M62>>().AddModule<ScaleModule<M63>>()
                        .AddModule<ScaleModule<M64>>().AddModule<ScaleModule<M65>>().AddModule<ScaleModule<M66>>()
                        .AddModule<ScaleModule<M67>>().AddModule<ScaleModule<M68>>().AddModule<ScaleModule<M69>>()
                        .AddModule<ScaleModule<M70>>().AddModule<ScaleModule<M71>>().AddModule<ScaleModule<M72>>()
                        .AddModule<ScaleModule<M73>>().AddModule<ScaleModule<M74>>().AddModule<ScaleModule<M75>>()
                        .AddModule<ScaleModule<M76>>().AddModule<ScaleModule<M77>>().AddModule<ScaleModule<M78>>()
                        .AddModule<ScaleModule<M79>>().AddModule<ScaleModule<M80>>().AddModule<ScaleModule<M81>>()
                        .AddModule<ScaleModule<M82>>().AddModule<ScaleModule<M83>>().AddModule<ScaleModule<M84>>()
                        .AddModule<ScaleModule<M85>>().AddModule<ScaleModule<M86>>().AddModule<ScaleModule<M87>>()
                        .AddModule<ScaleModule<M88>>().AddModule<ScaleModule<M89>>().AddModule<ScaleModule<M90>>()
                        .AddModule<ScaleModule<M91>>().AddModule<ScaleModule<M92>>().AddModule<ScaleModule<M93>>()
                        .AddModule<ScaleModule<M94>>().AddModule<ScaleModule<M95>>().AddModule<ScaleModule<M96>>()
                        .AddModule<ScaleModule<M97>>().AddModule<ScaleModule<M98>>().AddModule<ScaleModule<M99>>()
                        .AddModule<ScaleModule<M100>>()

                let! pipelineSummary = builder.ExecutePipelineAsync() |> Async.AwaitTask
                do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
                do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(tracker.CompletedCount()), expectedModuleCount))
            }

