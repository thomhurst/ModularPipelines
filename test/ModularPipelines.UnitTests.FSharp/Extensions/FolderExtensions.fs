namespace ModularPipelines.UnitTests.FSharp.Extensions

open System.Linq
open ModularPipelines.FileSystem

module FolderExtensions =
    let findAncestorContainingProject (original: Folder) =
        let rec findProject (folder: Folder option) =
            match folder with
            | None -> None
            | Some f ->
                let hasProject = 
                    f.ListFiles()
                    |> Seq.exists (fun x -> x.Extension = ".csproj" || x.Extension = ".fsproj")
                if hasProject then
                    Some f
                else
                    findProject (Option.ofObj f.Parent)
        findProject (Some original)

type FolderExtensions =
    [<System.Runtime.CompilerServices.Extension>]
    static member FindAncestorContainingProject(original: Folder) =
        FolderExtensions.findAncestorContainingProject original
        |> Option.toObj
