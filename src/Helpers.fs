[<AutoOpen>]
module Fs.Obsidian.Helpers

open System
open Browser.Types
open Fable.Core
open Fable.Core.JS
open Fable.Import
open ObsidianBindings

[<ImportAll("obsidian")>]
let obsidian: ObsidianBindings.IExports = jsNative

module Command =
    
    let private defaultCommand() =
        let mutable mid = ""
        let mutable mname = ""
        let mutable _cb = None
        let mutable _ccb = None
        let mutable _hotkeys = None
        let mutable _editorCallback = None
        let mutable _editorCheckCallback = None
        
        {
            new Command with
                member this.callback with get() = _cb and set v = _cb <- v
                member this.checkCallback with get() = _ccb and set v = _ccb <- v
                member this.editorCallback with get() = _editorCallback and set v = _editorCallback <- v
                member this.editorCheckCallback with get() = _editorCheckCallback and set v = _editorCheckCallback <- v
                member this.hotkeys with get() = _hotkeys and set v = _hotkeys <- v

                member this.icon: string option = None

                member this.icon
                    with set (v: string option): unit = ()

                member this.id: string = mid

                member this.id
                    with set (v: string): unit = mid <- v

                member this.mobileOnly: bool option = None

                member this.mobileOnly
                    with set (v: bool option): unit = ()

                member this.name: string = mname

                member this.name
                    with set (v: string): unit = mname <- v
        }
    
    let forMenu id name callback =
        let cmd = defaultCommand()
        cmd.id <- id
        cmd.name <- name
        cmd.callback <- Some callback
        cmd
        
    let forEditor id name callback =
        let cmd = defaultCommand()
        cmd.id <- id
        cmd.name <- name
        cmd.editorCallback <- Some callback
        cmd
        
    let forEditorCheck id name callback =
        let cmd = defaultCommand()
        cmd.id <- id
        cmd.name <- name
        cmd.editorCheckCallback <- Some callback
        cmd
        
        
let printJson x = x |> JSON.stringify |> printfn "%s"



module SuggestModal =
    
    type private t<'t> = SuggestModal<'t>
    
    open JsInterop
    
    let create app =
        obsidian.SuggestModal.Create(app)
        
    let withGetSuggestions (query:string -> ResizeArray<'t>) (sm:t<'t>) =
        sm?getSuggestions <- (fun f -> query f |> U2.Case1)
        sm
             
    let withOnChooseSuggestion (fn:'t->unit) (sm:t<'t>) =
        sm?onChooseSuggestion <- (fun f eventargs -> fn f )
        sm
    let withRenderSuggestion (fn:'t->HTMLDivElement->unit) (sm:t<'t>) =
        sm?renderSuggestion <-
            (fun sugg (elem:HTMLDivElement) ->
                fn sugg elem
                None
            )
        sm
    let withDefaultRenderSuggestion (sm:t<'t>) =
        sm?renderSuggestion <-
            (fun (sugg) (elem:HTMLDivElement) -> //:FuzzyMatch<string>
                elem.innerText <- JSON.stringify(sugg)
                elem |> Some
            )
        sm
            


module Content =
    
    type CodeBlockContent = { startLine: int; title:string option; content:string }
    
    let private om fn x = Option.map fn x
    let getCodeBlocks (app:App) =
        
        let view =
            obsidian.MarkdownView
            :?> Constructor<MarkdownView>
            |> app.workspace.getActiveViewOfType
        if view.IsNone then None else
            
        let view = view.Value
        let lines = view.getViewData().Split('\n')
        
        let codeBlockSections =
            app.workspace.getActiveFile()
            |> Option.map app.metadataCache.getFileCache
            |> Option.flatten
            |> Option.map (fun f ->
                f.sections
                |> Option.map (fun d -> 
                    d
                    |> Seq.where (fun d -> d.``type`` = "code" )
                )
            )
            |> Option.flatten
        
        let codeBlockTexts =
            codeBlockSections
            
            |> Option.map (fun optionalCodeblockSections ->
                optionalCodeblockSections
                |> Seq.map (fun f ->
                    let startLine = int f.position.start.line + 1
                    let endLine = int f.position.``end``.line - 1
                    let blockContent = lines.[startLine..endLine]
                    {
                        startLine = startLine
                        title =
                            blockContent
                            |> Seq.tryFind (fun f -> f.StartsWith "title:")
                            |> Option.map (fun f -> f.["title:".Length..].Trim())
                        content =
                            blockContent
                            |> Seq.where (fun f -> not (f.StartsWith "title:"))
                            |> String.concat "\n"
                    }
                )
                |> Seq.toArray
            )
        codeBlockTexts
        
    let getHeadings (app:App) =
        let view =
            obsidian.MarkdownView
            :?> Constructor<MarkdownView>
            |> app.workspace.getActiveViewOfType
        if view.IsNone then None else
            
        let view = view.Value
        let lines = view.getViewData().Split('\n')
        
        let codeBlockSections =
            app.workspace.getActiveFile()
            |> Option.bind app.metadataCache.getFileCache
            |> Option.bind (fun f ->
                f.headings
                |> Option.map (fun d -> 
                    d.ToArray()
                )
            )
        
        let codeBlockTexts =
            codeBlockSections
            
            |> Option.map (fun headingCaches ->
                headingCaches
                |> Seq.map (fun f ->
                    let startLine = int f.position.start.line + 1
                    let endLine = int f.position.``end``.line - 1
                    let blockContent = lines.[startLine..endLine]
                    {
                        startLine = startLine
                        title =
                            blockContent
                            |> Seq.tryFind (fun f -> f.StartsWith "title:")
                            |> Option.map (fun f -> f.["title:".Length..])
                        content =
                            blockContent
                            |> Seq.where (fun f -> not (f.StartsWith "title:"))
                            |> String.concat "\n"
                    }
                )
                |> Seq.toArray
            )
        codeBlockTexts
        
        
module Seq =
    let skipSafe (num: int) (source: seq<'a>) : seq<'a> =
        seq {
            use e = source.GetEnumerator()
            let mutable idx =  0
            let mutable loop =  true
            while idx < num && loop do
                if not(e.MoveNext()) then
                    loop <- false
                idx <- idx + 1

            while e.MoveNext() do
                yield e.Current 
        }
        
[<CLIMutable>]        
type PluginSettings =
    {
        defaultCodeBlockLanguage : string
    }
    static member Default = {
        defaultCodeBlockLanguage = ""
    }
     



module PluginSettings =
    let withDynamicProp (key:string) (value:string) (settings:PluginSettings) =
        match key with
        | "defaultCodeBlockLanguage" ->  { settings with defaultCodeBlockLanguage = value }
        | _ -> failwith $"unknown property {key}"
        
type ExtendedPlugin<'TSettings> =
    inherit Plugin_2
    abstract settings: 'TSettings with get, set
    abstract loadSettings: unit -> 'TSettings
    abstract saveSettings: 'TSettings -> Promise<unit>
    
    
module Clipboard =
    let write txt =
        match Browser.Navigator.navigator.clipboard with
        | None -> promise { () }
        | Some v -> v.writeText(txt) 
