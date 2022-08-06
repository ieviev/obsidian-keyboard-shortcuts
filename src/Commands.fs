module Fs.Obsidian.Commands

open System.Text.RegularExpressions
open Browser
open Fable.Core
open ObsidianBindings


let [<Literal>] headingregex = """^(#+) """
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
                            let text = f.title |> Option.defaultValue f.content
                            f, obsidian.fuzzySearch(query,text)
                        )
                        |> Seq.where (fun f -> snd f |> Option.isSome)
//                            |> Seq.sortBy (fun f -> (snd f).Value.score )
                        |> Seq.map fst
                    matches |> ResizeArray
                    )
                |> SuggestModal.withRenderSuggestion (fun f elem ->
                    elem.innerText <- f.title |> Option.defaultValue f.content)
                |> SuggestModal.withOnChooseSuggestion (fun f ->
                    $"copied:\n{f.content.Substring(0, min (f.content.Length) 50)}"
                    |> U2.Case1 |> obsidian.Notice.Create |> ignore
                    Clipboard.write f.content |> ignore
                )
                
            modal.``open``()
            None
        )
    
let rec createAdmonitionView (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditorCheck (nameof createAdmonitionView) "Create Code Block With Title"
        (fun check editor view ->
            
            printJson "edit called"
            
            let v =
                obsidian.MarkdownEditView
                :?> Constructor<MarkdownEditView>
                |> plugin.app.workspace.getActiveViewOfType
            
            printJson "edit called"
            
            let mutable result = ""
            let mutable textElement = null
            
            let modal =
                plugin.app
                |> obsidian.Modal.Create
        
            modal.contentEl.createEl("h1",U2.Case2 "What's your name?") |> ignore
            
            let typesettings =
                modal.contentEl
                |> obsidian.Setting.Create
                |> (fun f ->
                    f.setName(U2.Case1 "name") |> ignore
                    f.addText (fun txt ->
                        txt.onChange(fun value ->
                            result <- value
                            None
                            )|> ignore
                        textElement <- txt
                        None
                    )
                    |> ignore
                    f
                )
            
            
            modal.contentEl
            |> obsidian.Setting.Create
            |> (fun f ->
                f.addButton(fun btn ->
                    btn
                        .setButtonText("submit")
                        .setCta()
                        .onClick(fun _ ->
                            modal.close()
                            editor.replaceSelection(result)
                            None
                            )
                    |> ignore
                    None
                ) |> ignore
            )
            
            modal.``open``()
            textElement.inputEl.focus()
        
            U2.Case2 ()
        )
        
let rec insertHeading4 (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof insertHeading4) "Insert heading 4"
        (fun edit ->
            edit.replaceSelection("#### ")
            doNone
        )            
   
let rec insertAdmonitionInfo (plugin:ExtendedPlugin<PluginSettings>) =
    Command.forEditor (nameof insertAdmonitionInfo) "Insert Info Admonition"
        (fun edit ->
            edit.replaceSelection("````ad-info\ntitle: \n````")
            let cursor = edit.getCursor()
            edit.setCursor(U2.Case2 (cursor.line - 1.))
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
    
    Command.forEditor "testcomand" "qqqqqq"
        (fun edit ->
            edit.replaceSelection("````\nhello\n````")
            let cursor = edit.getCursor()
            edit.setCursor(U2.Case2 (cursor.line - 1.))
            doNone
        )