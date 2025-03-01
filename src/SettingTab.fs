module Fs.Obsidian.SettingTab

open Fable.Core
open Fs.Obsidian
open Microsoft.FSharp.Reflection
open ObsidianBindings
open Fable.Core.JsInterop
open System
open System.Text
open Fs.Obsidian.Gen


type String with 
    /// "defaultCalloutType" -> "Default callout type"
    static member camelcaseToHumanReadable (str:string) =
        str 
        |> Seq.pairwise 
        |> Seq.fold (fun (sb:StringBuilder) (c1:char,c2:char) -> 
            let up = (fun v -> Char.IsUpper v)
            match up c1, up c2 with 
            | _, _ when Char.IsDigit c2 -> 
                $" {c2} " |> sb.Append
            | false, true -> 
                $" {Char.ToLower c2}" |> sb.Append
            | _ -> c2 |> sb.Append
        ) (StringBuilder($"{Char.ToUpper(str[0])}"))
        |> string


let createSettingForProperty (plugin:ExtendedPlugin<PluginSettings>) (settingTab:SettingTab) (propName:string) (propType:Type) =
    let setting = obsidian.Setting.Create(settingTab.containerEl) .setName(propName |> String.camelcaseToHumanReadable |> U2.Case1)
    let createEditor() : unit = 
        if propType = typeof<bool> then
            setting.addToggle(fun toggle -> 
                let property =
                    typeof<PluginSettings>
                    |> FSharpType.GetRecordFields
                    |> Seq.find (fun f -> f.Name = propName)
                !!property.GetValue(plugin.settings) |> toggle.setValue |> ignore
                toggle.onChange(fun value ->
                    plugin.settings <-
                        plugin.settings
                        |> PluginSettings.setFieldByName propName value
                    None
                )
                |> ignore
                None
            ) |> ignore
        elif propType = typeof<string> then
            setting.addText(fun txt -> 
                let property =
                    typeof<PluginSettings>
                    |> FSharpType.GetRecordFields
                    |> Seq.find (fun f -> f.Name = propName)
                !!property.GetValue(plugin.settings) |> txt.setValue |> ignore
                txt.onChange(fun value ->
                    plugin.settings <-
                        plugin.settings
                        |> PluginSettings.setFieldByName propName value
                    None
                )
                |> ignore
                None
            ) |> ignore
        else ()
    
    createEditor()
    ()


let createSettingDisplay (plugin:ExtendedPlugin<PluginSettings>) (settingtab:PluginSettingTab) () =
    let containerEl = settingtab.containerEl
    containerEl.empty()
    containerEl.createEl( "h2",
        unbox U2.Case1 {| text = "Quick snippets and navigation" |}) |> ignore
    
    let fields = typeof<PluginSettings> |> FSharpType.GetRecordFields
    
    for field in fields do
        createSettingForProperty plugin settingtab field.Name field.PropertyType
    
    None
    

    
let create (app:App) (plugin:ExtendedPlugin<PluginSettings>) =
    let settingtab = obsidian.PluginSettingTab.Create(app,plugin)
    settingtab?display <- createSettingDisplay plugin settingtab
    settingtab?hide <- (fun f ->
        // printJson "saving settings"
        plugin.saveSettings(plugin.settings) |> ignore
        None    
    )
    settingtab
