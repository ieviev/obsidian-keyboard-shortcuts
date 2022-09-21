

open System
open Microsoft.FSharp.Reflection

type PluginSettings =
    {
        defaultCodeBlockLanguage : string
        setting1 : string
    }

let generateDynamicSet (t:Type) =
    let s4 = "    "
    let s8 = s4+s4
    t
    |> FSharpType.GetRecordFields
    |> Array.map (fun f ->
        $"{s8}| \"{f.Name}\" ->  {{ settings with {f.Name} = value }}"    
    )
    |> (fun f ->
        [
            $"module {t.Name} ="
            $"{s4}let withDynamicProp (key:string) (value:string) (settings:{t.Name}) ="
            $"{s8}match key with"
            yield! f
            $"{s8}| _ -> failwith $\"unknown property {{key}}\""
        ]
        |> String.concat "\n"
    )
    
// typeof<PluginSettings> |> generateDynamicSet |> stdout.WriteLine


module Manifest =

    open System
    open System.Collections.Generic
    open System.Text.Json

    let trimDesc (desc:string) = 
        desc |> (fun f -> f.Trim().Split("\n") |> Array.map (fun f -> f.Trim()) |> String.concat "\n")

    [<CLIMutable>]
    type Manifest = {
        id : string
        name : string
        version : string
        minAppVersion : string
        description : string
        author : string
        authorUrl : string
        isDesktopOnly : bool
    }


    let appmanifest = 
        {
            id = "quick-snippets-and-navigation"
            name = "Quick snippets and navigation"
            version = "1.0.2"
            minAppVersion = "0.13.33"
            description = trimDesc """
                Keyboard navigation up/down for headings
                - Configurable default code block and callout
                - Copy code block via keyboard shortcut
                """ 
            author = "aciq"
            authorUrl = "https://github.com/aciq/obsidian-keyboard-shortcuts"
            isDesktopOnly = false
        }

    open System.IO
    let genmanifest() =
        JsonSerializer.Serialize(appmanifest,
            JsonSerializerOptions(WriteIndented=true))
        |> (fun f -> 
            let p1 = __SOURCE_DIRECTORY__ + "/../dist/manifest.json"
            let p2 = __SOURCE_DIRECTORY__ + "/../manifest.json"
            File.WriteAllText(p1,f)
            File.WriteAllText(p2,f)
        )


// Manifest.genmanifest()