

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



let t1 = "#category/category1/categ2" 

let firstlevel = t1.Substring(0,t1.IndexOf("/"))

module String =
    /// e.g. "a/b/c/d" |> nthIndexOf 2 '/' returns 3
    let nthIndexOf (n:int) (char:char) (str:string) =
        let rec loop (pos:int) n =
            match str.IndexOf(char, pos) with
            | -1 -> -1
            | newIdx when n > 1 -> 
                loop (newIdx + 1) (n - 1)
            | newIdx -> newIdx
        loop 0 n

    // "a/b/c/d" |> String.untilNthOccurrence 2 '/' -> a/b/
    let untilNthOccurrence (n:int) (char:char) (str:string) =
        match str |> nthIndexOf (n) (char) with 
        | -1 -> str
        | n -> str.Substring(0,n + 1)

"a/b/c/d" |> String.nthIndexOf 3 '/'
"a/b/c/d" |> String.untilNthOccurrence 2 '/'
t1 |> String.untilNthOccurrence 2 '/'
t1 |> String.nthIndexOf 1 '/'
t1 |> String.nthIndexOf 0 '/'

let secondlevel = 
    let l1 = t1.IndexOf("/")
    let l2 = t1.IndexOf("/", (l1 + 1) )
    let level1 = t1.Substring(0,l2)
    level1