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
    [<Struct>] type C1 = struct end
    [<Struct>] type C2 = struct end
    [<Struct>] type C3 = struct end
    [<Struct>] type C4 = struct end
    [<Struct>] type C5 = struct end
    [<Struct>] type C6 = struct end
    [<Struct>] type C7 = struct end
    [<Struct>] type C8 = struct end
    [<Struct>] type C9 = struct end
    [<Struct>] type C10 = struct end
    [<Struct>] type C11 = struct end
    [<Struct>] type C12 = struct end
    [<Struct>] type C13 = struct end
    [<Struct>] type C14 = struct end
    [<Struct>] type C15 = struct end
    [<Struct>] type C16 = struct end
    [<Struct>] type C17 = struct end
    [<Struct>] type C18 = struct end
    [<Struct>] type C19 = struct end
    [<Struct>] type C20 = struct end
    [<Struct>] type C21 = struct end
    [<Struct>] type C22 = struct end
    [<Struct>] type C23 = struct end
    [<Struct>] type C24 = struct end
    [<Struct>] type C25 = struct end
    [<Struct>] type C26 = struct end
    [<Struct>] type C27 = struct end
    [<Struct>] type C28 = struct end
    [<Struct>] type C29 = struct end
    [<Struct>] type C30 = struct end
    [<Struct>] type C31 = struct end
    [<Struct>] type C32 = struct end
    [<Struct>] type C33 = struct end
    [<Struct>] type C34 = struct end
    [<Struct>] type C35 = struct end
    [<Struct>] type C36 = struct end
    [<Struct>] type C37 = struct end
    [<Struct>] type C38 = struct end
    [<Struct>] type C39 = struct end
    [<Struct>] type C40 = struct end
    [<Struct>] type C41 = struct end
    [<Struct>] type C42 = struct end
    [<Struct>] type C43 = struct end
    [<Struct>] type C44 = struct end
    [<Struct>] type C45 = struct end
    [<Struct>] type C46 = struct end
    [<Struct>] type C47 = struct end
    [<Struct>] type C48 = struct end
    [<Struct>] type C49 = struct end
    [<Struct>] type C50 = struct end

    // Chain modules - each depends on the previous one
    type ChainModule1(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain1")
                tracker.MarkCompleted("Chain1")
                return 1
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule1>)>]
    type ChainModule2(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain2")
                tracker.MarkCompleted("Chain2")
                return 2
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule2>)>]
    type ChainModule3(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain3")
                tracker.MarkCompleted("Chain3")
                return 3
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule3>)>]
    type ChainModule4(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain4")
                tracker.MarkCompleted("Chain4")
                return 4
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule4>)>]
    type ChainModule5(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain5")
                tracker.MarkCompleted("Chain5")
                return 5
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule5>)>]
    type ChainModule6(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain6")
                tracker.MarkCompleted("Chain6")
                return 6
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule6>)>]
    type ChainModule7(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain7")
                tracker.MarkCompleted("Chain7")
                return 7
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule7>)>]
    type ChainModule8(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain8")
                tracker.MarkCompleted("Chain8")
                return 8
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule8>)>]
    type ChainModule9(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain9")
                tracker.MarkCompleted("Chain9")
                return 9
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule9>)>]
    type ChainModule10(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain10")
                tracker.MarkCompleted("Chain10")
                return 10
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule10>)>]
    type ChainModule11(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain11")
                tracker.MarkCompleted("Chain11")
                return 11
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule11>)>]
    type ChainModule12(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain12")
                tracker.MarkCompleted("Chain12")
                return 12
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule12>)>]
    type ChainModule13(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain13")
                tracker.MarkCompleted("Chain13")
                return 13
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule13>)>]
    type ChainModule14(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain14")
                tracker.MarkCompleted("Chain14")
                return 14
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule14>)>]
    type ChainModule15(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain15")
                tracker.MarkCompleted("Chain15")
                return 15
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule15>)>]
    type ChainModule16(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain16")
                tracker.MarkCompleted("Chain16")
                return 16
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule16>)>]
    type ChainModule17(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain17")
                tracker.MarkCompleted("Chain17")
                return 17
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule17>)>]
    type ChainModule18(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain18")
                tracker.MarkCompleted("Chain18")
                return 18
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule18>)>]
    type ChainModule19(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain19")
                tracker.MarkCompleted("Chain19")
                return 19
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule19>)>]
    type ChainModule20(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain20")
                tracker.MarkCompleted("Chain20")
                return 20
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule20>)>]
    type ChainModule21(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain21")
                tracker.MarkCompleted("Chain21")
                return 21
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule21>)>]
    type ChainModule22(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain22")
                tracker.MarkCompleted("Chain22")
                return 22
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule22>)>]
    type ChainModule23(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain23")
                tracker.MarkCompleted("Chain23")
                return 23
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule23>)>]
    type ChainModule24(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain24")
                tracker.MarkCompleted("Chain24")
                return 24
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule24>)>]
    type ChainModule25(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain25")
                tracker.MarkCompleted("Chain25")
                return 25
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule25>)>]
    type ChainModule26(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain26")
                tracker.MarkCompleted("Chain26")
                return 26
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule26>)>]
    type ChainModule27(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain27")
                tracker.MarkCompleted("Chain27")
                return 27
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule27>)>]
    type ChainModule28(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain28")
                tracker.MarkCompleted("Chain28")
                return 28
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule28>)>]
    type ChainModule29(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain29")
                tracker.MarkCompleted("Chain29")
                return 29
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule29>)>]
    type ChainModule30(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain30")
                tracker.MarkCompleted("Chain30")
                return 30
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule30>)>]
    type ChainModule31(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain31")
                tracker.MarkCompleted("Chain31")
                return 31
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule31>)>]
    type ChainModule32(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain32")
                tracker.MarkCompleted("Chain32")
                return 32
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule32>)>]
    type ChainModule33(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain33")
                tracker.MarkCompleted("Chain33")
                return 33
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule33>)>]
    type ChainModule34(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain34")
                tracker.MarkCompleted("Chain34")
                return 34
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule34>)>]
    type ChainModule35(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain35")
                tracker.MarkCompleted("Chain35")
                return 35
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule35>)>]
    type ChainModule36(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain36")
                tracker.MarkCompleted("Chain36")
                return 36
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule36>)>]
    type ChainModule37(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain37")
                tracker.MarkCompleted("Chain37")
                return 37
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule37>)>]
    type ChainModule38(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain38")
                tracker.MarkCompleted("Chain38")
                return 38
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule38>)>]
    type ChainModule39(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain39")
                tracker.MarkCompleted("Chain39")
                return 39
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule39>)>]
    type ChainModule40(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain40")
                tracker.MarkCompleted("Chain40")
                return 40
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule40>)>]
    type ChainModule41(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain41")
                tracker.MarkCompleted("Chain41")
                return 41
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule41>)>]
    type ChainModule42(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain42")
                tracker.MarkCompleted("Chain42")
                return 42
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule42>)>]
    type ChainModule43(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain43")
                tracker.MarkCompleted("Chain43")
                return 43
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule43>)>]
    type ChainModule44(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain44")
                tracker.MarkCompleted("Chain44")
                return 44
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule44>)>]
    type ChainModule45(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain45")
                tracker.MarkCompleted("Chain45")
                return 45
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule45>)>]
    type ChainModule46(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain46")
                tracker.MarkCompleted("Chain46")
                return 46
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule46>)>]
    type ChainModule47(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain47")
                tracker.MarkCompleted("Chain47")
                return 47
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule47>)>]
    type ChainModule48(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain48")
                tracker.MarkCompleted("Chain48")
                return 48
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule48>)>]
    type ChainModule49(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain49")
                tracker.MarkCompleted("Chain49")
                return 49
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<ChainModule49>)>]
    type ChainModule50(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("Chain50")
                tracker.MarkCompleted("Chain50")
                return 50
            }
   
    // Root module for fan-out
    type FanOutRootModule(tracker: ExecutionTracker) =
        inherit Module<bool>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutRoot")
                tracker.MarkCompleted("FanOutRoot")
                return true
            }

    // Fan-out dependent modules - all depend on the root
    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep1(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep1")
                tracker.MarkCompleted("FanOutDep1")
                return 1
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep2(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep2")
                tracker.MarkCompleted("FanOutDep2")
                return 2
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep3(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep3")
                tracker.MarkCompleted("FanOutDep3")
                return 3
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep4(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep4")
                tracker.MarkCompleted("FanOutDep4")
                return 4
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep5(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep5")
                tracker.MarkCompleted("FanOutDep5")
                return 5
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep6(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep6")
                tracker.MarkCompleted("FanOutDep6")
                return 6
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep7(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep7")
                tracker.MarkCompleted("FanOutDep7")
                return 7
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep8(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep8")
                tracker.MarkCompleted("FanOutDep8")
                return 8
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep9(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep9")
                tracker.MarkCompleted("FanOutDep9")
                return 9
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep10(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep10")
                tracker.MarkCompleted("FanOutDep10")
                return 10
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep11(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep11")
                tracker.MarkCompleted("FanOutDep11")
                return 11
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep12(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep12")
                tracker.MarkCompleted("FanOutDep12")
                return 12
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep13(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep13")
                tracker.MarkCompleted("FanOutDep13")
                return 13
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep14(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep14")
                tracker.MarkCompleted("FanOutDep14")
                return 14
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep15(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep15")
                tracker.MarkCompleted("FanOutDep15")
                return 15
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep16(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep16")
                tracker.MarkCompleted("FanOutDep16")
                return 16
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep17(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep17")
                tracker.MarkCompleted("FanOutDep17")
                return 17
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep18(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep18")
                tracker.MarkCompleted("FanOutDep18")
                return 18
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep19(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep19")
                tracker.MarkCompleted("FanOutDep19")
                return 19
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep20(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep20")
                tracker.MarkCompleted("FanOutDep20")
                return 20
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep21(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep21")
                tracker.MarkCompleted("FanOutDep21")
                return 21
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep22(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep22")
                tracker.MarkCompleted("FanOutDep22")
                return 22
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep23(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep23")
                tracker.MarkCompleted("FanOutDep23")
                return 23
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep24(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep24")
                tracker.MarkCompleted("FanOutDep24")
                return 24
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep25(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep25")
                tracker.MarkCompleted("FanOutDep25")
                return 25
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep26(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep26")
                tracker.MarkCompleted("FanOutDep26")
                return 26
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep27(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep27")
                tracker.MarkCompleted("FanOutDep27")
                return 27
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep28(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep28")
                tracker.MarkCompleted("FanOutDep28")
                return 28
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep29(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep29")
                tracker.MarkCompleted("FanOutDep29")
                return 29
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep30(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep30")
                tracker.MarkCompleted("FanOutDep30")
                return 30
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep31(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep31")
                tracker.MarkCompleted("FanOutDep31")
                return 31
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep32(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep32")
                tracker.MarkCompleted("FanOutDep32")
                return 32
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep33(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep33")
                tracker.MarkCompleted("FanOutDep33")
                return 33
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep34(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep34")
                tracker.MarkCompleted("FanOutDep34")
                return 34
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep35(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep35")
                tracker.MarkCompleted("FanOutDep35")
                return 35
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep36(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep36")
                tracker.MarkCompleted("FanOutDep36")
                return 36
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep37(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep37")
                tracker.MarkCompleted("FanOutDep37")
                return 37
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep38(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep38")
                tracker.MarkCompleted("FanOutDep38")
                return 38
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep39(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep39")
                tracker.MarkCompleted("FanOutDep39")
                return 39
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep40(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep40")
                tracker.MarkCompleted("FanOutDep40")
                return 40
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep41(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep41")
                tracker.MarkCompleted("FanOutDep41")
                return 41
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep42(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep42")
                tracker.MarkCompleted("FanOutDep42")
                return 42
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep43(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep43")
                tracker.MarkCompleted("FanOutDep43")
                return 43
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep44(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep44")
                tracker.MarkCompleted("FanOutDep44")
                return 44
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep45(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep45")
                tracker.MarkCompleted("FanOutDep45")
                return 45
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep46(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep46")
                tracker.MarkCompleted("FanOutDep46")
                return 46
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep47(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep47")
                tracker.MarkCompleted("FanOutDep47")
                return 47
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep48(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep48")
                tracker.MarkCompleted("FanOutDep48")
                return 48
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep49(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep49")
                tracker.MarkCompleted("FanOutDep49")
                return 49
            }

    [<ModularPipelines.Attributes.DependsOn(typeof<FanOutRootModule>)>]
    type FanOutDep50(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanOutDep50")
                tracker.MarkCompleted("FanOutDep50")
                return 50
            }

    // Independent modules for fan-in
    type FanInInd1(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd1")
                tracker.MarkCompleted("FanInInd1")
                return 1
            }

    type FanInInd2(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd2")
                tracker.MarkCompleted("FanInInd2")
                return 2
            }

    type FanInInd3(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd3")
                tracker.MarkCompleted("FanInInd3")
                return 3
            }

    type FanInInd4(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd4")
                tracker.MarkCompleted("FanInInd4")
                return 4
            }

    type FanInInd5(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd5")
                tracker.MarkCompleted("FanInInd5")
                return 5
            }

    type FanInInd6(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd6")
                tracker.MarkCompleted("FanInInd6")
                return 6
            }

    type FanInInd7(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd7")
                tracker.MarkCompleted("FanInInd7")
                return 7
            }

    type FanInInd8(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd8")
                tracker.MarkCompleted("FanInInd8")
                return 8
            }

    type FanInInd9(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd9")
                tracker.MarkCompleted("FanInInd9")
                return 9
            }

    type FanInInd10(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd10")
                tracker.MarkCompleted("FanInInd10")
                return 10
            }

    type FanInInd11(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd11")
                tracker.MarkCompleted("FanInInd11")
                return 11
            }

    type FanInInd12(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd12")
                tracker.MarkCompleted("FanInInd12")
                return 12
            }

    type FanInInd13(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd13")
                tracker.MarkCompleted("FanInInd13")
                return 13
            }

    type FanInInd14(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd14")
                tracker.MarkCompleted("FanInInd14")
                return 14
            }

    type FanInInd15(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd15")
                tracker.MarkCompleted("FanInInd15")
                return 15
            }

    type FanInInd16(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd16")
                tracker.MarkCompleted("FanInInd16")
                return 16
            }

    type FanInInd17(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd17")
                tracker.MarkCompleted("FanInInd17")
                return 17
            }

    type FanInInd18(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd18")
                tracker.MarkCompleted("FanInInd18")
                return 18
            }

    type FanInInd19(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd19")
                tracker.MarkCompleted("FanInInd19")
                return 19
            }

    type FanInInd20(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd20")
                tracker.MarkCompleted("FanInInd20")
                return 20
            }

    type FanInInd21(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd21")
                tracker.MarkCompleted("FanInInd21")
                return 21
            }

    type FanInInd22(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd22")
                tracker.MarkCompleted("FanInInd22")
                return 22
            }

    type FanInInd23(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd23")
                tracker.MarkCompleted("FanInInd23")
                return 23
            }

    type FanInInd24(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd24")
                tracker.MarkCompleted("FanInInd24")
                return 24
            }

    type FanInInd25(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd25")
                tracker.MarkCompleted("FanInInd25")
                return 25
            }

    type FanInInd26(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd26")
                tracker.MarkCompleted("FanInInd26")
                return 26
            }

    type FanInInd27(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd27")
                tracker.MarkCompleted("FanInInd27")
                return 27
            }

    type FanInInd28(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd28")
                tracker.MarkCompleted("FanInInd28")
                return 28
            }

    type FanInInd29(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd29")
                tracker.MarkCompleted("FanInInd29")
                return 29
            }

    type FanInInd30(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd30")
                tracker.MarkCompleted("FanInInd30")
                return 30
            }

    type FanInInd31(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd31")
                tracker.MarkCompleted("FanInInd31")
                return 31
            }

    type FanInInd32(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd32")
                tracker.MarkCompleted("FanInInd32")
                return 32
            }

    type FanInInd33(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd33")
                tracker.MarkCompleted("FanInInd33")
                return 33
            }

    type FanInInd34(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd34")
                tracker.MarkCompleted("FanInInd34")
                return 34
            }

    type FanInInd35(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd35")
                tracker.MarkCompleted("FanInInd35")
                return 35
            }

    type FanInInd36(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd36")
                tracker.MarkCompleted("FanInInd36")
                return 36
            }

    type FanInInd37(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd37")
                tracker.MarkCompleted("FanInInd37")
                return 37
            }

    type FanInInd38(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd38")
                tracker.MarkCompleted("FanInInd38")
                return 38
            }

    type FanInInd39(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd39")
                tracker.MarkCompleted("FanInInd39")
                return 39
            }

    type FanInInd40(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd40")
                tracker.MarkCompleted("FanInInd40")
                return 40
            }

    type FanInInd41(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd41")
                tracker.MarkCompleted("FanInInd41")
                return 41
            }

    type FanInInd42(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd42")
                tracker.MarkCompleted("FanInInd42")
                return 42
            }

    type FanInInd43(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd43")
                tracker.MarkCompleted("FanInInd43")
                return 43
            }

    type FanInInd44(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd44")
                tracker.MarkCompleted("FanInInd44")
                return 44
            }

    type FanInInd45(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd45")
                tracker.MarkCompleted("FanInInd45")
                return 45
            }

    type FanInInd46(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd46")
                tracker.MarkCompleted("FanInInd46")
                return 46
            }

    type FanInInd47(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd47")
                tracker.MarkCompleted("FanInInd47")
                return 47
            }

    type FanInInd48(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd48")
                tracker.MarkCompleted("FanInInd48")
                return 48
            }

    type FanInInd49(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd49")
                tracker.MarkCompleted("FanInInd49")
                return 49
            }

    type FanInInd50(tracker: ExecutionTracker) =
        inherit Module<int>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInInd50")
                tracker.MarkCompleted("FanInInd50")
                return 50
            }

    // Final module that depends on all independent modules
    [<ModularPipelines.Attributes.DependsOn(typeof<FanInInd1>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd2>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd3>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd4>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd5>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd6>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd7>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd8>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd9>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd10>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd11>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd12>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd13>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd14>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd15>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd16>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd17>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd18>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd19>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd20>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd21>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd22>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd23>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd24>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd25>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd26>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd27>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd28>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd29>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd30>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd31>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd32>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd33>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd34>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd35>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd36>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd37>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd38>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd39>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd40>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd41>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd42>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd43>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd44>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd45>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd46>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd47>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd48>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd49>);
      ModularPipelines.Attributes.DependsOn(typeof<FanInInd50>)>]
    type FanInFinalModule(tracker: ExecutionTracker) =
        inherit Module<bool>()

        override _.ExecuteAsync(context: IModuleContext, cancellationToken: CancellationToken) =
            task {
                tracker.RecordStart("FanInFinal")
                tracker.MarkCompleted("FanInFinal")
                return true
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
       
        /// <summary>
        /// Verifies that a pipeline with a 50-module deep dependency chain
        /// executes modules in the correct order.
        /// </summary>
        /// <remarks>
        /// This test validates:
        /// - Deep dependency chains are resolved correctly
        /// - Modules execute in dependency order
        /// - No deadlocks occur with long chains
        /// </remarks>
        [<Test>]
        member _.Pipeline_With50ModuleDeepChain_CompletesInOrder() =
         async{
            let tracker = new ExecutionTracker();
            do! check(Assert.That<bool>(tracker.IsClean).IsTrue())
            let chainDepth = 50;
            let builder =
                TestPipelineHostBuilder.Create()
                    .ConfigureServices(fun _ services -> services.AddSingleton(tracker) |> ignore)
                    .AddModule<ChainModule1>().AddModule<ChainModule2>().AddModule<ChainModule3>()
                    .AddModule<ChainModule4>().AddModule<ChainModule5>().AddModule<ChainModule6>()
                    .AddModule<ChainModule7>().AddModule<ChainModule8>().AddModule<ChainModule9>()
                    .AddModule<ChainModule10>().AddModule<ChainModule11>().AddModule<ChainModule12>()
                    .AddModule<ChainModule13>().AddModule<ChainModule14>().AddModule<ChainModule15>()
                    .AddModule<ChainModule16>().AddModule<ChainModule17>().AddModule<ChainModule18>()
                    .AddModule<ChainModule19>().AddModule<ChainModule20>().AddModule<ChainModule21>()
                    .AddModule<ChainModule22>().AddModule<ChainModule23>().AddModule<ChainModule24>()
                    .AddModule<ChainModule25>().AddModule<ChainModule26>().AddModule<ChainModule27>()
                    .AddModule<ChainModule28>().AddModule<ChainModule29>().AddModule<ChainModule30>()
                    .AddModule<ChainModule31>().AddModule<ChainModule32>().AddModule<ChainModule33>()
                    .AddModule<ChainModule34>().AddModule<ChainModule35>().AddModule<ChainModule36>()
                    .AddModule<ChainModule37>().AddModule<ChainModule38>().AddModule<ChainModule39>()
                    .AddModule<ChainModule40>().AddModule<ChainModule41>().AddModule<ChainModule42>()
                    .AddModule<ChainModule43>().AddModule<ChainModule44>().AddModule<ChainModule45>()
                    .AddModule<ChainModule46>().AddModule<ChainModule47>().AddModule<ChainModule48>()
                    .AddModule<ChainModule49>().AddModule<ChainModule50>();
            let! pipelineSummary = builder.ExecutePipelineAsync() |> Async.AwaitTask
            do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
            do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(tracker.CompletedCount()), chainDepth))

            // Verify all chain modules executed
            for i = 1 to chainDepth do
                let record = tracker.GetRecord($"Chain{i}")
                do! check(Assert.That(record.IsSome).IsTrue())

            // Verify dependency ordering: each module must complete before its dependent
            // Chain1 -> Chain2 -> Chain3 -> ... -> Chain50
            // Get all records sorted by completion order for diagnostics
            let orderedRecords =
                [ 1 .. chainDepth ]
                |> List.choose (fun i -> tracker.GetRecord($"Chain{i}"))
                |> List.sortBy (fun r -> r.CompletionOrder)

            // Verify the completion order matches the dependency order
            for i, actualRecord in orderedRecords |> List.indexed do
                let expectedModuleName = $"Chain{i + 1}"

                do! check(
                    StringEqualsAssertionExtensions.IsEqualTo(
                        Assert.That(actualRecord.ModuleName),
                        expectedModuleName
                    ).Because($"Module at completion order {i + 1} should be {expectedModuleName} but was {actualRecord.ModuleName}")
                )
         }

        /// <summary>
        /// Verifies that a fan-out pattern (1 root with 50 dependents) executes correctly.
        /// </summary>
        /// <remarks>
        /// This test validates:
        /// - All 50 dependent modules wait for the root to complete
        /// - Dependent modules can execute in parallel after the root completes
        /// - No race conditions with many modules depending on one
        /// </remarks>
        [<Test>]
        member _.Pipeline_With1ModuleAnd50Dependents_CompletesSuccessfully() =
            async {
                let tracker = new ExecutionTracker()
                do! check(Assert.That<bool>(tracker.IsClean).IsTrue())
                let totalModules = 51

                let builder =
                    TestPipelineHostBuilder.Create()
                        .ConfigureServices(fun _ services -> services.AddSingleton(tracker) |> ignore)
                        .AddModule<FanOutRootModule>()
                        .AddModule<FanOutDep1>().AddModule<FanOutDep2>().AddModule<FanOutDep3>()
                        .AddModule<FanOutDep4>().AddModule<FanOutDep5>().AddModule<FanOutDep6>()
                        .AddModule<FanOutDep7>().AddModule<FanOutDep8>().AddModule<FanOutDep9>()
                        .AddModule<FanOutDep10>().AddModule<FanOutDep11>().AddModule<FanOutDep12>()
                        .AddModule<FanOutDep13>().AddModule<FanOutDep14>().AddModule<FanOutDep15>()
                        .AddModule<FanOutDep16>().AddModule<FanOutDep17>().AddModule<FanOutDep18>()
                        .AddModule<FanOutDep19>().AddModule<FanOutDep20>().AddModule<FanOutDep21>()
                        .AddModule<FanOutDep22>().AddModule<FanOutDep23>().AddModule<FanOutDep24>()
                        .AddModule<FanOutDep25>().AddModule<FanOutDep26>().AddModule<FanOutDep27>()
                        .AddModule<FanOutDep28>().AddModule<FanOutDep29>().AddModule<FanOutDep30>()
                        .AddModule<FanOutDep31>().AddModule<FanOutDep32>().AddModule<FanOutDep33>()
                        .AddModule<FanOutDep34>().AddModule<FanOutDep35>().AddModule<FanOutDep36>()
                        .AddModule<FanOutDep37>().AddModule<FanOutDep38>().AddModule<FanOutDep39>()
                        .AddModule<FanOutDep40>().AddModule<FanOutDep41>().AddModule<FanOutDep42>()
                        .AddModule<FanOutDep43>().AddModule<FanOutDep44>().AddModule<FanOutDep45>()
                        .AddModule<FanOutDep46>().AddModule<FanOutDep47>().AddModule<FanOutDep48>()
                        .AddModule<FanOutDep49>().AddModule<FanOutDep50>()

                let! pipelineSummary = builder.ExecutePipelineAsync() |> Async.AwaitTask

                do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
                do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(tracker.CompletedCount()), totalModules))

                let rootRecord = tracker.GetRecord("FanOutRoot")
                do! check(Assert.That(rootRecord.IsSome).IsTrue())

                for i = 1 to 50 do
                    let dependent = tracker.GetRecord($"FanOutDep{i}")
                    do! check(Assert.That(dependent.IsSome).IsTrue())
            }

        /// <summary>
        /// Verifies that a fan-in pattern (50 independent modules + 1 final) executes correctly.
        /// </summary>
        /// <remarks>
        /// This test validates:
        /// - The final module waits for all 50 independent modules to complete
        /// - Independent modules can execute in parallel
        /// - No race conditions with one module depending on many
        /// </remarks>
        [<Test>]
        member _.Pipeline_With50ModulesAndOneFinalModule_CompletesSuccessfully() =
            async {
                let tracker = new ExecutionTracker()
                do! check(Assert.That<bool>(tracker.IsClean).IsTrue())
                let totalModules = 51

                let builder =
                    TestPipelineHostBuilder.Create()
                        .ConfigureServices(fun _ services -> services.AddSingleton(tracker) |> ignore)
                        .AddModule<FanInInd1>().AddModule<FanInInd2>().AddModule<FanInInd3>()
                        .AddModule<FanInInd4>().AddModule<FanInInd5>().AddModule<FanInInd6>()
                        .AddModule<FanInInd7>().AddModule<FanInInd8>().AddModule<FanInInd9>()
                        .AddModule<FanInInd10>().AddModule<FanInInd11>().AddModule<FanInInd12>()
                        .AddModule<FanInInd13>().AddModule<FanInInd14>().AddModule<FanInInd15>()
                        .AddModule<FanInInd16>().AddModule<FanInInd17>().AddModule<FanInInd18>()
                        .AddModule<FanInInd19>().AddModule<FanInInd20>().AddModule<FanInInd21>()
                        .AddModule<FanInInd22>().AddModule<FanInInd23>().AddModule<FanInInd24>()
                        .AddModule<FanInInd25>().AddModule<FanInInd26>().AddModule<FanInInd27>()
                        .AddModule<FanInInd28>().AddModule<FanInInd29>().AddModule<FanInInd30>()
                        .AddModule<FanInInd31>().AddModule<FanInInd32>().AddModule<FanInInd33>()
                        .AddModule<FanInInd34>().AddModule<FanInInd35>().AddModule<FanInInd36>()
                        .AddModule<FanInInd37>().AddModule<FanInInd38>().AddModule<FanInInd39>()
                        .AddModule<FanInInd40>().AddModule<FanInInd41>().AddModule<FanInInd42>()
                        .AddModule<FanInInd43>().AddModule<FanInInd44>().AddModule<FanInInd45>()
                        .AddModule<FanInInd46>().AddModule<FanInInd47>().AddModule<FanInInd48>()
                        .AddModule<FanInInd49>().AddModule<FanInInd50>()
                        .AddModule<FanInFinalModule>()

                let! pipelineSummary = builder.ExecutePipelineAsync() |> Async.AwaitTask

                do! check(Assert.That(pipelineSummary.Status).IsEqualTo(Status.Successful))
                do! check(IntEqualsAssertionExtensions.IsEqualTo(Assert.That(tracker.CompletedCount()), totalModules))

                let finalRecord = tracker.GetRecord("FanInFinal")
                do! check(Assert.That(finalRecord.IsSome).IsTrue())

                for i = 1 to 50 do
                    let independent = tracker.GetRecord($"FanInInd{i}")
                    do! check(Assert.That(independent.IsSome).IsTrue())
            }