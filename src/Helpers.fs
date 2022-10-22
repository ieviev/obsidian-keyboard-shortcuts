[<AutoOpen>]
module Fs.Obsidian.Helpers

open System
open Browser.Types
open Fable.Core
open Fable.Core.JS
open Fable.Import
open ObsidianBindings
open JsInterop
open System.Collections.Generic

[<ImportAll("obsidian")>]
let obsidian: ObsidianBindings.IExports = jsNative

module Command =

    let private defaultCommand () =
        let mutable mid = ""
        let mutable mname = ""
        let mutable _cb = None
        let mutable _ccb = None
        let mutable _hotkeys = None
        let mutable _editorCallback = None
        let mutable _editorCheckCallback = None

        { new Command with
            member this.callback
                with get () = _cb
                and set v = _cb <- v

            member this.checkCallback
                with get () = _ccb
                and set v = _ccb <- v

            member this.editorCallback
                with get () = _editorCallback
                and set v = _editorCallback <- v

            member this.editorCheckCallback
                with get () = _editorCheckCallback
                and set v = _editorCheckCallback <- v

            member this.hotkeys
                with get () = _hotkeys
                and set v = _hotkeys <- v

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
                with set (v: string): unit = mname <- v }

    let forMenu id name callback =
        let cmd = defaultCommand ()
        cmd.id <- id
        cmd.name <- name
        cmd.callback <- Some callback
        cmd

    let forEditor id name callback =
        let cmd = defaultCommand ()
        cmd.id <- id
        cmd.name <- name
        cmd.editorCallback <- Some(callback)
        cmd

    let forEditorCheck id name callback =
        let cmd = defaultCommand ()
        cmd.id <- id
        cmd.name <- name
        cmd.editorCheckCallback <- Some callback
        cmd


let printJson x = x |> JSON.stringify |> printfn "%s"


type SuggestModal<'t> with
    member this.currentSelection : 't =
        let values: 't [] = this?chooser?values
        values[this?chooser?selectedItem]

module SuggestModal =
    
    [<AutoOpen>]
    type SuggestModalKeyboardShortcut<'t> =
        { modifiers: Modifier seq
          key: string
          action: (KeyboardEvent * SuggestModal<'t>) -> unit }

    open JsInterop

    let create app = obsidian.SuggestModal.Create(app)

    let withGetSuggestions (query: string -> ResizeArray<'t>) (sm: SuggestModal<'t>) =
        sm?getSuggestions <- (fun f -> query f |> U2.Case1)
        sm

    let withGetSuggestions2 (query: string -> seq<'t>) (sm: SuggestModal<'t>) =
        sm?getSuggestions <- (fun f -> query f |> ResizeArray |> U2.Case1)
        sm

    /// overrides OnChooseSuggestion and does not close modal
    let withOnSelectSuggestion (fn: 't * (U2<MouseEvent, KeyboardEvent>) -> unit) (sm: SuggestModal<'t>) =
        sm?selectSuggestion <- (fun f eventargs -> fn (f, eventargs))
        sm

    let withOnChooseSuggestion (fn: 't * (U2<MouseEvent, KeyboardEvent>) -> unit) (sm: SuggestModal<'t>) =
        sm?onChooseSuggestion <- (fun f eventargs -> fn (f, eventargs))
        sm

    let withRenderSuggestion (fn: 't -> HTMLDivElement -> unit) (sm: SuggestModal<'t>) =
        sm?renderSuggestion <- (fun sugg (elem: HTMLDivElement) ->
            fn sugg elem
            None)

        sm

    let withDefaultRenderSuggestion (sm: SuggestModal<'t>) =
        sm?renderSuggestion <- (fun (sugg) (elem: HTMLDivElement) -> //:FuzzyMatch<string>
            elem.innerText <- JSON.stringify (sugg)
            elem |> Some)

        sm


    let withKeyboardShortcut
        (keyboardShortcut: SuggestModalKeyboardShortcut<'t>)
        (sm: SuggestModal<'t>)
        =
        sm.scope.register (
            keyboardShortcut.modifiers |> ResizeArray,
            Some keyboardShortcut.key,
            !!(fun (evt: KeyboardEvent) ->
                keyboardShortcut.action (evt,sm)
                U2.Case1 false)
        )
        |> ignore

        sm

    let map (mapping) (sm: SuggestModal<'t>) =
        mapping sm
        sm


    /// <summary>
    /// instructions -> (command * purpose) list <br/>
    /// line 2
    /// </summary>
    let withInstructions (instructions: (string * string) list) (sm: SuggestModal<'t>) =
        let instructions': ResizeArray<Instruction> =
            instructions
            |> List.map (fun (command, purpose) ->
                !!{| command = command
                     purpose = purpose |})
            |> ResizeArray

        sm.setInstructions (instructions')
        sm


    let mapResultContainer (map) (sm: SuggestModal<'t>) =
        map sm.resultContainerEl
        sm

    let mapBackgroundContainer (mapping) (sm: SuggestModal<'t>) =
        mapping sm.containerEl
        sm

    let openModal (sm: SuggestModal<'t>) = sm.``open`` ()


module Notice =
    let show (text: string) =
        text
        |> U2.Case1
        |> obsidian.Notice.Create
        |> ignore

module Content =
    open System.Collections.Generic

    type CodeBlockContent = { startLine: int; content: string }

    let private om fn x = Option.map fn x

    let getTags (app: App) =
        app.workspace.getActiveFile ()
        |> Option.bind app.metadataCache.getFileCache
        |> Option.bind (fun f -> f.tags)


    


    let getCodeBlocks (app: App) =

        let view =
            obsidian.MarkdownView :?> Constructor<MarkdownView>
            |> app.workspace.getActiveViewOfType

        if view.IsNone then
            None
        else

            let view = view.Value
            let lines = view.getViewData().Split('\n')

            let codeBlockSections =
                app.workspace.getActiveFile ()
                |> Option.map app.metadataCache.getFileCache
                |> Option.flatten
                |> Option.map (fun f ->
                    f.sections
                    |> Option.map (fun d -> d |> Seq.where (fun d -> d.``type`` = "code")))
                |> Option.flatten

            let codeBlockTexts =
                codeBlockSections

                |> Option.map (fun optionalCodeblockSections ->
                    optionalCodeblockSections
                    |> Seq.map (fun f ->
                        let startLine = int f.position.start.line + 1
                        let endLine = int f.position.``end``.line - 1
                        let blockContent = lines.[startLine..endLine]

                        { startLine = startLine
                          content = blockContent |> String.concat "\n" })
                    |> Seq.toArray)

            codeBlockTexts

    let getHeadings (app: App) =
        let view =
            obsidian.MarkdownView :?> Constructor<MarkdownView>
            |> app.workspace.getActiveViewOfType

        if view.IsNone then
            None
        else

            let view = view.Value
            let lines = view.getViewData().Split('\n')

            let codeBlockSections =
                app.workspace.getActiveFile ()
                |> Option.bind app.metadataCache.getFileCache
                |> Option.bind (fun f -> f.headings |> Option.map (fun d -> d.ToArray()))

            let codeBlockTexts =
                codeBlockSections

                |> Option.map (fun headingCaches ->
                    headingCaches
                    |> Seq.map (fun f ->
                        let startLine = int f.position.start.line + 1
                        let endLine = int f.position.``end``.line - 1
                        let blockContent = lines.[startLine..endLine]

                        { startLine = startLine
                          content =
                            blockContent
                            |> Seq.where (fun f -> not (f.StartsWith "title:"))
                            |> String.concat "\n" })
                    |> Seq.toArray)

            codeBlockTexts


module Seq =
    let skipSafe (num: int) (source: seq<'a>) : seq<'a> =
        seq {
            use e = source.GetEnumerator()
            let mutable idx = 0
            let mutable loop = true

            while idx < num && loop do
                if not (e.MoveNext()) then loop <- false
                idx <- idx + 1

            while e.MoveNext() do
                yield e.Current
        }





type ExtendedPlugin<'TSettings> =
    inherit Plugin_2
    abstract settings: 'TSettings with get, set
    abstract loadSettings: unit -> 'TSettings
    abstract saveSettings: 'TSettings -> Promise<unit>


module Clipboard =
    let write txt =
        match Browser.Navigator.navigator.clipboard with
        | None -> promise { () }
        | Some v -> v.writeText (txt)


module String =
    /// e.g. "a/b/c/d" |> nthIndexOf 2 '/' returns 3
    let nthIndexOf (n: int) (char: char) (str: string) =
        let rec loop (pos: int) n =
            match str.IndexOf(char, pos) with
            | -1 -> -1
            | newIdx when n > 1 -> loop (newIdx + 1) (n - 1)
            | newIdx -> newIdx

        loop 0 n

    // "a/b/c/d" |> String.untilNthOccurrence 2 '/' -> a/b/
    let untilNthOccurrence (n: int) (char: char) (str: string) =
        match str |> nthIndexOf (n) (char) with
        | -1 -> str
        | n -> str.Substring(0, n + 1)




type App with

    member this.getAllTags() : obj =
        this.metadataCache?getTags()
        
    
    member this.executeCommandById (commandId:string) : bool =
        this?commands?executeCommandById(commandId)

    member this.getTagsOfFile(file:TFile) =
        let fileCacheOpt = this.metadataCache.getFileCache(file)

        match fileCacheOpt with 
        | None -> Seq.empty
        | Some cache -> 
            let tags1 = 
                cache.tags
                |> Option.map (Seq.map (fun f -> f.tag))
                |> Option.defaultValue Seq.empty

            let tags2 : seq<string> = 
                cache.frontmatter
                |> Option.map (fun f ->
                    let tags = !!f.["tags"]
                    if tags = null then Seq.empty
                    else tags
                )
                |> Option.defaultValue Seq.empty
                |> Seq.map (fun f -> $"#{f}")

            Seq.append tags1 tags2

      