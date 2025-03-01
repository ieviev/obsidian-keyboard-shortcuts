[<AutoOpen>]
module Fs.Obsidian.Helpers

open Browser.Types
open Fable.Core
open Fable.Core.JS
open ObsidianBindings
open JsInterop

[<ImportAll("obsidian")>]
let obsidian: IExports = jsNative

module Command =

    let forMenu id name icon callback =
        jsOptions<Command> (fun v ->
            v.id <- id
            v.name <- name
            v.icon <- Some icon
            v.callback <- Some callback
        )


    let forEditor id name icon callback =
        jsOptions<Command> (fun v ->
            v.id <- id
            v.name <- name
            v.icon <- Some icon
            v.editorCallback <- Some callback
        )


let printJson x = x |> JSON.stringify |> printfn "%s"


type SuggestModal<'t> with

    member this.currentSelection: 't =
        let values: 't[] = this?chooser?values
        values[this?chooser?selectedItem]

module SuggestModal =

    [<AutoOpen>]
    type SuggestModalKeyboardShortcut<'t> = {
        modifiers: Modifier seq
        key: string
        action: (KeyboardEvent * SuggestModal<'t>) -> unit
    }

    let create app = obsidian.SuggestModal.Create(app)

    let withGetSuggestions (query: string -> ResizeArray<'t>) (sm: SuggestModal<'t>) =
        sm?getSuggestions <- (fun f -> query f |> U2.Case1)

        sm

    let withGetSuggestions2 (query: string -> seq<'t>) (sm: SuggestModal<'t>) =
        sm?getSuggestions <- (fun f -> query f |> ResizeArray |> U2.Case1)

        sm

    /// overrides OnChooseSuggestion and does not close modal
    let withOnSelectSuggestion
        (fn: 't * (U2<MouseEvent, KeyboardEvent>) -> unit)
        (sm: SuggestModal<'t>)
        =
        sm?selectSuggestion <- (fun f eventargs -> fn (f, eventargs))
        sm

    let withOnChooseSuggestion
        (fn: 't * (U2<MouseEvent, KeyboardEvent>) -> unit)
        (sm: SuggestModal<'t>)
        =
        sm?onChooseSuggestion <- (fun f eventargs -> fn (f, eventargs))
        sm

    let withRenderSuggestion (fn: 't -> HTMLDivElement -> unit) (sm: SuggestModal<'t>) =
        sm?renderSuggestion <-
            (fun sugg (elem: HTMLDivElement) ->
                fn sugg elem
                None
            )

        sm

    let withDefaultRenderSuggestion (sm: SuggestModal<'t>) =
        sm?renderSuggestion <-
            (fun (sugg) (elem: HTMLDivElement) -> //:FuzzyMatch<string>
                elem.innerText <- JSON.stringify (sugg)

                elem |> Some
            )

        sm


    let withKeyboardShortcut
        (keyboardShortcut: SuggestModalKeyboardShortcut<'t>)
        (sm: SuggestModal<'t>)
        =
        sm.scope.register (
            keyboardShortcut.modifiers |> ResizeArray,
            Some keyboardShortcut.key,
            !!(fun (evt: KeyboardEvent) ->
                keyboardShortcut.action (evt, sm)
                U2.Case1 false
            )
        )
        |> ignore

        sm

    /// helper to use ctrl and mod both
    let inline withCtrlKeyboardShortcut
        (keyboardShortcut: SuggestModalKeyboardShortcut<'t>)
        (sm: SuggestModal<'t>)
        =
        sm.scope.register (
            ResizeArray([ Modifier.Mod ]),
            Some keyboardShortcut.key,
            !!(fun (evt: KeyboardEvent) ->
                keyboardShortcut.action (evt, sm)
                U2.Case1 false
            )
        )
        |> ignore

        sm.scope.register (
            ResizeArray [ Modifier.Ctrl ],
            Some keyboardShortcut.key,
            !!(fun (evt: KeyboardEvent) ->
                keyboardShortcut.action (evt, sm)
                U2.Case1 false
            )
        )
        |> ignore

        sm

    let inline map mapping (sm: SuggestModal<'t>) =
        mapping sm
        sm


    /// <summary>
    /// instructions -> (command * purpose) list <br/>
    /// line 2
    /// </summary>
    let inline withInstructions (instructions: (string * string) list) (sm: SuggestModal<'t>) =
        let instructions': ResizeArray<Instruction> =
            instructions
            |> List.map (fun (command, purpose) ->
                !!{|
                    command = command
                    purpose = purpose
                |}
            )
            |> ResizeArray

        sm.setInstructions (instructions')
        sm


    let inline mapResultContainer (map) (sm: SuggestModal<'t>) =
        map sm.resultContainerEl
        sm

    let inline mapBackgroundContainer (mapping) (sm: SuggestModal<'t>) =
        mapping sm.containerEl
        sm

    let inline openModal (sm: SuggestModal<'t>) = sm.``open`` ()


module Notice =
    let inline show (text: string) =
        text |> U2.Case1 |> obsidian.Notice.Create |> ignore

module Content =

    type CodeBlockContent = {
        startLine: int
        endLine: int
        content: string
    }

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

            let codeBlockSections: SectionCache seq option =
                app.workspace.getActiveFile ()
                |> Option.bind app.metadataCache.getFileCache
                |> Option.bind (fun f ->
                    f.sections
                    |> Option.map (fun d -> d |> Seq.where (fun d -> d.``type`` = "code"))
                )

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
                            endLine = endLine
                            content = blockContent |> String.concat "\n"
                        }
                    )
                    |> Seq.toArray
                )

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

                        {
                            startLine = startLine
                            endLine = endLine
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
            let mutable idx = 0
            let mutable loop = true

            while idx < num && loop do
                if not (e.MoveNext()) then
                    loop <- false

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
    let inline write txt =
        match Browser.Navigator.navigator.clipboard with
        | None -> promise { () }
        | Some v -> v.writeText (txt)


module String =
    /// "a/b/c/d" |> nthIndexOf 2 '/' -> 3
    let inline nthIndexOf (n: int) (char: char) (str: string) =
        let rec loop (pos: int) n =
            match str.IndexOf(char, pos) with
            | -1 -> -1
            | newIdx when n > 1 -> loop (newIdx + 1) (n - 1)
            | newIdx -> newIdx

        loop 0 n

    // "a/b/c/d" |> String.untilNthOccurrence 2 '/' -> a/b/
    let inline untilNthOccurrence (n: int) (char: char) (str: string) =
        match str |> nthIndexOf (n) (char) with
        | -1 -> str
        | n -> str.Substring(0, n + 1)


type App with

    member this.getAllTags() : obj = this.metadataCache?getTags ()


    member this.executeCommandById(commandId: string) : bool =
        this?commands?executeCommandById (commandId)

    member this.getTagsOfFile(file: TFile) =
        let fileCacheOpt = this.metadataCache.getFileCache (file)

        match fileCacheOpt with
        | None -> Seq.empty
        | Some cache ->
            let tags1 =
                cache.tags
                |> Option.map (Seq.map (fun f -> f.tag))
                |> Option.defaultValue Seq.empty

            let tags2: seq<string> =
                cache.frontmatter
                |> Option.map (fun f ->
                    let tags = !!f.["tags"]
                    if tags = null then Seq.empty else tags
                )
                |> Option.defaultValue Seq.empty
                |> Seq.map (fun f -> $"#{f}")

            Seq.append tags1 tags2
