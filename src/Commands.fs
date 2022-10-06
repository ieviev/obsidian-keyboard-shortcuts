module Fs.Obsidian.Commands

open System.Text.RegularExpressions
open Browser
open Fable.Core
open ObsidianBindings
open Fable.Core.JsInterop


let [<Literal>] headingregex = """^(#{1,6}) """
let doNone = (fun f -> None)

let rec goToPrevHeading (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof goToPrevHeading) "Go to previous heading"
        (fun editor ->
            let cursor = editor.getCursor()
            let linesbefore =
                editor.getValue().Split('\n').[..int cursor.line] |> Array.rev
            
            let foundOpt = 
                linesbefore
                |> Seq.skipSafe 1
                |> Seq.tryFindIndex(fun f ->
                    Regex.Match(f,headingregex).Success )
            
            match foundOpt with
            | None -> doNone
            | Some moveby ->
                let newpos = int cursor.line - moveby - 1 |> float
                editor.setCursor(U2.Case2 newpos)
                doNone
        )
        
let rec goToNextHeading (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof goToNextHeading) "Go to next heading"
        (fun editor ->
            let cursor = editor.getCursor()
            let linesafter = editor.getValue().Split('\n').[int cursor.line..]
            
            let foundOpt = 
                linesafter
                |> Seq.skipSafe 1
                |> Seq.tryFindIndex(fun f ->
                    Regex.Match(f,headingregex).Success )
            
            match foundOpt with
            | None -> doNone
            | Some moveby ->
                let newpos = int cursor.line + moveby + 1 |> float
                editor.setCursor(U2.Case2 newpos)
                doNone
        )

let rec copyNextCodeBlock (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof copyNextCodeBlock) "Copy Next Code Block"
        (fun edit ->
            match plugin.app |> Content.getCodeBlocks with
            | None -> doNone
            | Some blocks ->  
                let cursor = edit.getCursor()
                blocks
                |> Seq.tryFind (fun f -> f.startLine > (int cursor.line))
                |> function
                    | None -> obsidian.Notice.Create(U2.Case1 "could not find a code block") |> ignore 
                    | Some v -> 
                        $"copied:\n{v.content.Substring(0, min (v.content.Length) 50)}"
                        |> U2.Case1 |> obsidian.Notice.Create |> ignore
                        Clipboard.write v.content |> ignore
                       
                        
                doNone
        )

let rec copyCodeBlock (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forMenu (nameof copyCodeBlock) "Copy Code Block"
        (fun _ ->
            let codeblocks = plugin.app |> Content.getCodeBlocks
            if codeblocks.IsNone then None else
                
            let codeblocks = codeblocks.Value
            
            let modal =
                plugin.app
                |> SuggestModal.create
                |> SuggestModal.withGetSuggestions (fun queryInput ->
                    let query = obsidian.prepareQuery queryInput
                    
                    let matches =
                        codeblocks
                        |> Seq.map (fun f ->
                            let text = f.content
                            f, obsidian.fuzzySearch(query,text)
                        )
                        |> Seq.where (fun f -> snd f |> Option.isSome)
//                            |> Seq.sortBy (fun f -> (snd f).Value.score )
                        |> Seq.map fst
                    matches |> ResizeArray
                    )
                |> SuggestModal.withRenderSuggestion (fun f elem ->
                    elem.innerText <- f.content)
                |> SuggestModal.withOnChooseSuggestion (fun f ->
                    $"copied:\n{f.content.Substring(0, min (f.content.Length) 50)}"
                    |> U2.Case1 |> obsidian.Notice.Create |> ignore
                    Clipboard.write f.content |> ignore
                )
                
            modal.``open``()
            None
        )
    
let rec openSwitcherWithTag1 (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forMenu (nameof openSwitcherWithTag1) "Open Switcher with Tag 1"
        (fun _ ->
            match plugin.app |> Content.getTags with 
            | None -> Notice.show "no tags found"
            | Some tags when tags.Count = 0 -> Notice.show "no tags found"
            | Some tags -> 
                let cmd = plugin.settings.defaultModalCommand
                match plugin.app?commands?executeCommandById(cmd) with 
                | false -> 
                    Notice.show $"failed to run command: {cmd}, configure Default modal command in settings"
                | true -> 
                    let modalInput = document.querySelector("body > div.modal-container > div.prompt > input")
                    modalInput?value <- $"%s{tags[0].tag} "
                    let ev = Browser.Event.Event.Create("input",null)
                    modalInput.dispatchEvent(ev) |> ignore
            None
        )    
        
let rec insertHeading4 (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof insertHeading4) "Insert heading 4"
        (fun edit ->
            edit.replaceSelection("#### ")
            doNone
        )            
   
let rec insertDefaultCallout (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof insertDefaultCallout) "Insert Default Callout"
        (fun edit ->
            edit.replaceSelection($"> [!{plugin.settings.defaultCalloutType}] ")
            doNone
        )   
let rec insertCodeBlock (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof insertCodeBlock) "Insert Code Block"
        (fun edit ->
            edit.replaceSelection($"````{plugin.settings.defaultCodeBlockLanguage}\n\n````")
            let cursor = edit.getCursor()
            edit.setCursor(U2.Case2 (cursor.line - 1.))
            doNone
        )  
        
let rec insertTest (plugin:ExtendedPlugin<PluginSettings>) =
    
    Command.forEditor "testtesttest" "qqqqqq"
        (fun edit ->
            edit.replaceSelection("````\naaaaa\n````")
            let cursor = edit.getCursor()
            edit.setCursor(U2.Case2 (cursor.line - 1.))
            doNone
        )