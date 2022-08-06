

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



open System
open System.Collections.Generic
open System.Text.Json

[<CLIMutable>]
type Manifest = {
    /// e.g. "nr-4.utils"
    id : string
    /// e.g. "Shortcuts"
    name : string
    /// e.g. "1.0.0"
    version : string
    /// e.g. "0.13.33"
    minAppVersion : string
    /// e.g. ""
    description : string
    /// e.g. "nr-4"
    author : string
    /// e.g. ""
    authorUrl : string
    /// e.g. false
    isDesktopOnly : bool
}


let appmanifest = 
    {
        id = "aciq.obsidian-keyboard-shortcuts"
        name = "Obsidian Keyboard Shortcuts"
        version = "1.0.0"
        minAppVersion = "0.13.33"
        description = ""
        author = "aciq"
        authorUrl = "https://github.com/aciq/obsidian-keyboard-shortcuts"
        isDesktopOnly = false
    }

open System.IO
let genmanifest() =
    appmanifest
    |> JsonSerializer.Serialize
    |> (fun f -> File.WriteAllText("./dist/manifest.json",f))