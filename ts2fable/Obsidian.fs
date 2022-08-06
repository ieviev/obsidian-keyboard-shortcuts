// ts2fable 0.7.1
module rec Obsidian
open System
open Fable.Core
open Fable.Core.JS
open Browser.Types

type Array<'T> = System.Collections.Generic.IList<'T>
type Function = System.Action
type RegExp = System.Text.RegularExpressions.Regex

module CodeMirror = Codemirror
type Extension = @codemirror_state.Extension
type StateField = @codemirror_state.StateField
type EditorView = @codemirror_view.EditorView
let [<Import("apiVersion","obsidian")>] apiVersion: string = jsNative
let [<Import("editorEditorField","obsidian")>] editorEditorField: StateField<EditorView> = jsNative
let [<Import("editorLivePreviewField","obsidian")>] editorLivePreviewField: StateField<bool> = jsNative
let [<Import("editorViewField","obsidian")>] editorViewField: StateField<MarkdownView> = jsNative
let [<Import("moment_2","obsidian")>] moment_2: obj = jsNative
let [<Import("Platform","obsidian")>] Platform: Platform = jsNative

type [<AllowNullLiteral>] IExports =
    abstract isBoolean: obj: obj option -> bool
    abstract fish: selector: string -> HTMLElement option
    abstract fishAll: selector: string -> ResizeArray<HTMLElement>
    abstract createEl: tag: 'K * ?o: U2<DomElementInfo, string> * ?callback: (HTMLElementTagNameMap -> unit) -> HTMLElementTagNameMap
    abstract createDiv: ?o: U2<DomElementInfo, string> * ?callback: (HTMLDivElement -> unit) -> HTMLDivElement
    abstract createSpan: ?o: U2<DomElementInfo, string> * ?callback: (HTMLSpanElement -> unit) -> HTMLSpanElement
    abstract createFragment: ?callback: (DocumentFragment -> unit) -> DocumentFragment
    abstract ajax: options: AjaxOptions -> unit
    abstract ajaxPromise: options: AjaxOptions -> Promise<obj option>
    abstract ready: fn: (unit -> obj option) -> unit
    abstract AbstractTextComponent: AbstractTextComponentStatic
    /// <summary>Adds an icon to the library</summary>
    /// <param name="iconId">- the icon ID</param>
    /// <param name="svgContent">- the content of the SVG, without the <svg>. Must fit viewBox="0 0 100 100".</param>
    abstract addIcon: iconId: string * svgContent: string -> unit
    abstract App: AppStatic
    abstract BaseComponent: BaseComponentStatic
    abstract ButtonComponent: ButtonComponentStatic
    abstract Component: ComponentStatic
    abstract Constructor: ConstructorStatic
    /// <summary>A standard debounce function.</summary>
    /// <param name="cb">- The function to call.</param>
    /// <param name="timeout">- The timeout to wait.</param>
    /// <param name="resetTimer">- Whether to reset the timeout when the debouncer is called again.</param>
    abstract debounce: cb: (obj -> obj option) * ?timeout: float * ?resetTimer: bool -> Debouncer<'T>
    abstract DropdownComponent: DropdownComponentStatic
    abstract EditableFileView: EditableFileViewStatic
    abstract Editor: EditorStatic
    abstract EditorSuggest: EditorSuggestStatic
    abstract Events: EventsStatic
    abstract ExtraButtonComponent: ExtraButtonComponentStatic
    abstract FileManager: FileManagerStatic
    abstract FileSystemAdapter: FileSystemAdapterStatic
    abstract FileView: FileViewStatic
    /// Flush the MathJax stylesheet.
    abstract finishRenderMath: unit -> Promise<unit>
    abstract fuzzySearch: q: PreparedQuery * text: string -> SearchResult option
    abstract FuzzySuggestModal: FuzzySuggestModalStatic
    abstract getAllTags: cache: CachedMetadata -> ResizeArray<string> option
    abstract getLinkpath: linktext: string -> string
    abstract HoverPopover: HoverPopoverStatic
    /// Converts HTML to Markdown using Turndown Service.
    abstract htmlToMarkdown: html: string -> string
    abstract ItemView: ItemViewStatic
    /// Iterate links and embeds. If callback returns true, the iteration process will be interrupted.
    abstract iterateCacheRefs: cache: CachedMetadata * cb: (ReferenceCache -> U2<bool, unit>) -> bool
    abstract iterateRefs: refs: ResizeArray<ReferenceCache> * cb: (ReferenceCache -> U2<bool, unit>) -> bool
    abstract Keymap: KeymapStatic
    /// Load MathJax.
    abstract loadMathJax: unit -> Promise<unit>
    /// Load Mermaid and return a promise to the global mermaid object.
    /// Can also use `mermaid` after this promise resolves to get the same reference.
    abstract loadMermaid: unit -> Promise<obj option>
    /// Load PDF.js and return a promise to the global pdfjsLib object.
    /// Can also use `window.pdfjsLib` after this promise resolves to get the same reference.
    abstract loadPdfJs: unit -> Promise<obj option>
    /// Load Prism.js and return a promise to the global Prism object.
    /// Can also use `Prism` after this promise resolves to get the same reference.
    abstract loadPrism: unit -> Promise<obj option>
    abstract MarkdownEditView: MarkdownEditViewStatic
    abstract MarkdownPreviewRenderer: MarkdownPreviewRendererStatic
    abstract MarkdownPreviewView: MarkdownPreviewViewStatic
    abstract MarkdownRenderChild: MarkdownRenderChildStatic
    abstract MarkdownRenderer: MarkdownRendererStatic
    abstract MarkdownSourceView: MarkdownSourceViewStatic
    abstract MarkdownView: MarkdownViewStatic
    abstract Menu: MenuStatic
    abstract MenuItem: MenuItemStatic
    abstract MetadataCache: MetadataCacheStatic
    abstract Modal: ModalStatic
    abstract MomentFormatComponent: MomentFormatComponentStatic
    abstract normalizePath: path: string -> string
    abstract Notice: NoticeStatic
    abstract parseFrontMatterAliases: frontmatter: obj option option -> ResizeArray<string> option
    abstract parseFrontMatterEntry: frontmatter: obj option option * key: U2<string, RegExp> -> obj option option
    abstract parseFrontMatterStringArray: frontmatter: obj option option * key: U2<string, RegExp> * ?nospaces: bool -> ResizeArray<string> option
    abstract parseFrontMatterTags: frontmatter: obj option option -> ResizeArray<string> option
    abstract parseLinktext: linktext: string -> ParseLinktextReturn
    abstract parseYaml: yaml: string -> obj option
    abstract Plugin_2: Plugin_2Static
    abstract PluginSettingTab: PluginSettingTabStatic
    abstract PopoverSuggest: PopoverSuggestStatic
    /// <summary>Construct a fuzzy search callback that runs on a target string.
    /// Performance may be an issue if you are running the search for more than a few thousand times.
    /// If performance is a problem, consider using `prepareSimpleSearch` instead.</summary>
    /// <param name="query">- the fuzzy query.</param>
    abstract prepareFuzzySearch: query: string -> (string -> SearchResult option)
    abstract prepareQuery: query: string -> PreparedQuery
    /// <summary>Construct a simple search callback that runs on a target string.</summary>
    /// <param name="query">- the space-separated words</param>
    abstract prepareSimpleSearch: query: string -> (string -> SearchResult option)
    abstract renderMatches: el: U2<HTMLElement, DocumentFragment> * text: string * matches: SearchMatches option * ?offset: float -> unit
    /// Render some LaTeX math using the MathJax engine. Returns an HTMLElement.
    /// Requires calling `finishRenderMath` when rendering is all done to flush the MathJax stylesheet.
    abstract renderMath: source: string * display: bool -> HTMLElement
    abstract renderResults: el: HTMLElement * text: string * result: SearchResult * ?offset: float -> unit
    /// Similar to `fetch()`, request a URL using HTTP/HTTPS, without any CORS restrictions.
    /// Returns the text value of the response.
    abstract request: request: RequestUrlParam -> Promise<string>
    /// Similar to `fetch()`, request a URL using HTTP/HTTPS, without any CORS restrictions.
    abstract requestUrl: request: RequestUrlParam -> Promise<RequestUrlResponse>
    /// Returns true if the API version is equal or higher than the requested version.
    /// Use this to limit functionality that require specific API versions to avoid
    /// crashing on older Obsidian builds.
    abstract requireApiVersion: version: string -> bool
    abstract resolveSubpath: cache: CachedMetadata * subpath: string -> U2<HeadingSubpathResult, BlockSubpathResult>
    abstract sanitizeHTMLToDom: html: string -> DocumentFragment
    abstract Scope: ScopeStatic
    abstract SearchComponent: SearchComponentStatic
    /// <param name="parent">- the HTML element to insert the icon</param>
    /// <param name="iconId">- the icon ID</param>
    /// <param name="size">- the pixel size for width and height, defaults to 16</param>
    abstract setIcon: parent: HTMLElement * iconId: string * ?size: float -> unit
    abstract Setting: SettingStatic
    abstract SettingTab: SettingTabStatic
    abstract SliderComponent: SliderComponentStatic
    abstract sortSearchResults: results: ResizeArray<SearchResultContainer> -> unit
    abstract stringifyYaml: obj: obj option -> string
    /// This function normalizes headings for link matching by stripping out special characters and shrinking consecutive spaces.
    abstract stripHeading: heading: string -> string
    abstract SuggestModal: SuggestModalStatic
    abstract TAbstractFile: TAbstractFileStatic
    abstract Tasks: TasksStatic
    abstract TextAreaComponent: TextAreaComponentStatic
    abstract TextComponent: TextComponentStatic
    abstract TextFileView: TextFileViewStatic
    abstract TFile: TFileStatic
    abstract TFolder: TFolderStatic
    abstract ToggleComponent: ToggleComponentStatic
    abstract ValueComponent: ValueComponentStatic
    abstract Vault: VaultStatic
    abstract View: ViewStatic
    abstract Workspace: WorkspaceStatic
    abstract WorkspaceItem: WorkspaceItemStatic
    abstract WorkspaceLeaf: WorkspaceLeafStatic
    abstract WorkspaceMobileDrawer: WorkspaceMobileDrawerStatic
    abstract WorkspaceParent: WorkspaceParentStatic
    abstract WorkspaceRibbon: WorkspaceRibbonStatic
    abstract WorkspaceSidedock: WorkspaceSidedockStatic
    abstract WorkspaceSplit: WorkspaceSplitStatic
    abstract WorkspaceTabs: WorkspaceTabsStatic

type [<AllowNullLiteral>] ParseLinktextReturn =
    abstract path: string with get, set
    abstract subpath: string with get, set

type [<AllowNullLiteral>] ObjectConstructor =
    abstract isEmpty: ``object``: Record<string, obj option> -> bool
    abstract each: ``object``: ObjectConstructorEachObject * callback: ('T -> string -> U2<bool, unit>) * ?context: obj -> bool
    abstract assign: target: obj option * [<ParamArray>] sources: obj option -> obj option
    abstract entries: obj: obj option -> ResizeArray<obj option>

type [<AllowNullLiteral>] ObjectConstructorEachObject =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> 'T with get, set

type [<AllowNullLiteral>] Array<'T> =
    abstract first: unit -> 'T option
    abstract last: unit -> 'T option
    abstract contains: target: 'T -> bool
    abstract remove: target: 'T -> unit
    abstract shuffle: unit -> Array<'T>

type [<AllowNullLiteral>] Math =
    abstract clamp: value: float * min: float * max: float -> float
    abstract square: value: float -> float

type [<AllowNullLiteral>] StringConstructor =
    abstract isString: obj: obj option -> bool

type [<AllowNullLiteral>] String =
    abstract contains: target: string -> bool
    abstract startsWith: searchString: string * ?position: float -> bool
    abstract endsWith: target: string * ?length: float -> bool
    abstract format: [<ParamArray>] args: ResizeArray<string> -> string

type [<AllowNullLiteral>] NumberConstructor =
    abstract isNumber: obj: obj option -> bool

type [<AllowNullLiteral>] Window =
    inherit EventTarget
    inherit AnimationFrameProvider
    inherit GlobalEventHandlers
    inherit WindowEventHandlers
    inherit WindowLocalStorage
    inherit WindowOrWorkerGlobalScope
    inherit WindowSessionStorage
    abstract isBoolean: obj: obj option -> bool
    abstract fish: selector: string -> HTMLElement option
    abstract fishAll: selector: string -> ResizeArray<HTMLElement>
    abstract ElementList: obj option with get, set

type [<AllowNullLiteral>] Element =
    inherit Node
    abstract getText: unit -> string
    abstract setText: ``val``: U2<string, DocumentFragment> -> unit
    abstract addClass: [<ParamArray>] classes: ResizeArray<string> -> unit
    abstract addClasses: classes: ResizeArray<string> -> unit
    abstract removeClass: [<ParamArray>] classes: ResizeArray<string> -> unit
    abstract removeClasses: classes: ResizeArray<string> -> unit
    abstract toggleClass: classes: U2<string, ResizeArray<string>> * value: bool -> unit
    abstract hasClass: cls: string -> bool
    abstract setAttr: qualifiedName: string * value: U3<string, float, bool> option -> unit
    abstract setAttrs: obj: ElementSetAttrsObj -> unit
    abstract getAttr: qualifiedName: string -> string option
    abstract matchParent: selector: string * ?lastParent: Element -> Element option
    abstract find: selector: string -> Element option
    abstract findAll: selector: string -> ResizeArray<HTMLElement>
    abstract findAllSelf: selector: string -> ResizeArray<HTMLElement>

type [<AllowNullLiteral>] ElementSetAttrsObj =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> U3<string, float, bool> option with get, set

type [<AllowNullLiteral>] Node =
    abstract detach: unit -> unit
    abstract empty: unit -> unit
    abstract insertAfter: other: Node -> unit
    abstract indexOf: other: Node -> float
    abstract setChildrenInPlace: children: ResizeArray<Node> -> unit
    abstract appendText: ``val``: string -> unit
    /// Create an element and append it to this node.
    abstract createEl: tag: 'K * ?o: U2<DomElementInfo, string> * ?callback: (HTMLElementTagNameMap -> unit) -> HTMLElementTagNameMap
    abstract createDiv: ?o: U2<DomElementInfo, string> * ?callback: (HTMLDivElement -> unit) -> HTMLDivElement
    abstract createSpan: ?o: U2<DomElementInfo, string> * ?callback: (HTMLSpanElement -> unit) -> HTMLSpanElement

type [<AllowNullLiteral>] HTMLElement =
    inherit Element
    abstract show: unit -> unit
    abstract hide: unit -> unit
    abstract toggle: show: bool -> unit
    abstract toggleVisibility: visible: bool -> unit
    /// Returns whether this element is shown, when the element is attached to the DOM and
    /// none of the parent and ancestor elements are hidden with `display: none`.
    /// 
    /// Exception: Does not work on <body> and <html>, or on elements with `position: fixed`.
    abstract isShown: unit -> bool
    abstract find: selector: string -> HTMLElement
    abstract findAll: selector: string -> ResizeArray<HTMLElement>
    abstract findAllSelf: selector: string -> ResizeArray<HTMLElement>
    abstract _EVENTS: obj option with get, set
    abstract on: this: HTMLElement * ``type``: 'K * selector: string * listener: (HTMLElement -> HTMLElementEventMap -> HTMLElement -> obj option) * ?options: U2<bool, AddEventListenerOptions> -> unit
    abstract off: this: HTMLElement * ``type``: 'K * selector: string * listener: (HTMLElement -> HTMLElementEventMap -> HTMLElement -> obj option) * ?options: U2<bool, AddEventListenerOptions> -> unit
    abstract onClickEvent: this: HTMLElement * listener: (HTMLElement -> MouseEvent -> obj option) * ?options: U2<bool, AddEventListenerOptions> -> unit
    /// <param name="listener">- the callback to call when this node is inserted into the DOM.</param>
    /// <param name="once">- if true, this will only fire once and then unhook itself.</param>
    abstract onNodeInserted: this: HTMLElement * listener: (unit -> obj option) * ?once: bool -> (unit -> unit)
    abstract trigger: eventType: string -> unit

type [<AllowNullLiteral>] DomElementInfo =
    /// The class to be assigned. Can be a space-separated string or an array of strings.
    abstract cls: U2<string, ResizeArray<string>> option with get, set
    /// The textContent to be assigned.
    abstract text: U2<string, DocumentFragment> option with get, set
    /// HTML attributes to be added.
    abstract attr: DomElementInfoAttr option with get, set
    /// HTML title (for hover tooltip).
    abstract title: string option with get, set
    /// The parent element to be assigned to.
    abstract parent: Node option with get, set
    abstract value: string option with get, set
    abstract ``type``: string option with get, set
    abstract prepend: bool option with get, set
    abstract href: string option with get, set

type [<AllowNullLiteral>] Document =
    abstract _EVENTS: obj option with get, set
    abstract on: this: Document * ``type``: 'K * selector: string * listener: (Document -> DocumentEventMap -> HTMLElement -> obj option) * ?options: U2<bool, AddEventListenerOptions> -> unit
    abstract off: this: Document * ``type``: 'K * selector: string * listener: (Document -> DocumentEventMap -> HTMLElement -> obj option) * ?options: U2<bool, AddEventListenerOptions> -> unit

type [<AllowNullLiteral>] EventListenerInfo =
    abstract selector: string with get, set
    abstract listener: Function with get, set
    abstract options: U2<bool, AddEventListenerOptions> option with get, set
    abstract callback: Function with get, set

type [<AllowNullLiteral>] AjaxOptions =
    abstract ``method``: AjaxOptionsMethod option with get, set
    abstract url: string with get, set
    abstract success: (obj option -> XMLHttpRequest -> obj option) option with get, set
    abstract error: (obj option -> XMLHttpRequest -> obj option) option with get, set
    abstract data: U3<obj, string, ArrayBuffer> option with get, set
    abstract headers: Record<string, string> option with get, set
    abstract withCredentials: bool option with get, set
    abstract req: XMLHttpRequest option with get, set

type [<AllowNullLiteral>] AbstractTextComponent<'T> =
    inherit ValueComponent<string>
    abstract inputEl: 'T with get, set
    abstract setDisabled: disabled: bool -> AbstractTextComponent<'T>
    abstract getValue: unit -> string
    abstract setValue: value: string -> AbstractTextComponent<'T>
    abstract setPlaceholder: placeholder: string -> AbstractTextComponent<'T>
    abstract onChanged: unit -> unit
    abstract onChange: callback: (string -> obj option) -> AbstractTextComponent<'T>

type [<AllowNullLiteral>] AbstractTextComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: inputEl: 'T -> AbstractTextComponent<'T>

type [<AllowNullLiteral>] App =
    abstract keymap: Keymap with get, set
    abstract scope: Scope with get, set
    abstract workspace: Workspace with get, set
    abstract vault: Vault with get, set
    abstract metadataCache: MetadataCache with get, set
    abstract fileManager: FileManager with get, set
    /// The last known user interaction event, to help commands find out what modifier keys are pressed.
    abstract lastEvent: UserEvent option with get, set

type [<AllowNullLiteral>] AppStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> App

type [<AllowNullLiteral>] BaseComponent =
    abstract disabled: bool with get, set
    /// Facilitates chaining
    abstract ``then``: cb: (BaseComponent -> obj option) -> BaseComponent
    abstract setDisabled: disabled: bool -> BaseComponent

type [<AllowNullLiteral>] BaseComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> BaseComponent

type [<AllowNullLiteral>] BlockCache =
    inherit CacheItem
    abstract id: string with get, set

type [<AllowNullLiteral>] BlockSubpathResult =
    inherit SubpathResult
    abstract ``type``: string with get, set
    abstract block: BlockCache with get, set
    abstract list: ListItemCache option with get, set

type [<AllowNullLiteral>] ButtonComponent =
    inherit BaseComponent
    abstract buttonEl: HTMLButtonElement with get, set
    abstract setDisabled: disabled: bool -> ButtonComponent
    abstract setCta: unit -> ButtonComponent
    abstract removeCta: unit -> ButtonComponent
    abstract setWarning: unit -> ButtonComponent
    abstract setTooltip: tooltip: string -> ButtonComponent
    abstract setButtonText: name: string -> ButtonComponent
    abstract setIcon: icon: string -> ButtonComponent
    abstract setClass: cls: string -> ButtonComponent
    abstract onClick: callback: (MouseEvent -> obj option) -> ButtonComponent

type [<AllowNullLiteral>] ButtonComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: containerEl: HTMLElement -> ButtonComponent

type [<AllowNullLiteral>] CachedMetadata =
    abstract links: ResizeArray<LinkCache> option with get, set
    abstract embeds: ResizeArray<EmbedCache> option with get, set
    abstract tags: ResizeArray<TagCache> option with get, set
    abstract headings: ResizeArray<HeadingCache> option with get, set
    /// Sections are root level markdown blocks, which can be used to divide the document up.
    abstract sections: ResizeArray<SectionCache> option with get, set
    abstract listItems: ResizeArray<ListItemCache> option with get, set
    abstract frontmatter: FrontMatterCache option with get, set
    abstract blocks: Record<string, BlockCache> option with get, set

type [<AllowNullLiteral>] CacheItem =
    abstract position: Pos with get, set

type [<AllowNullLiteral>] CloseableComponent =
    abstract close: unit -> obj option

type [<AllowNullLiteral>] Command =
    /// Globally unique ID to identify this command.
    abstract id: string with get, set
    /// Human friendly name for searching.
    abstract name: string with get, set
    /// Icon ID to be used in the toolbar.
    abstract icon: string option with get, set
    abstract mobileOnly: bool option with get, set
    /// Simple callback, triggered globally.
    abstract callback: (unit -> obj option) option with get, set
    /// Complex callback, overrides the simple callback.
    /// Used to "check" whether your command can be performed in the current circumstances.
    /// For example, if your command requires the active focused pane to be a MarkdownSourceView, then
    /// you should only return true if the condition is satisfied. Returning false or undefined causes
    /// the command to be hidden from the command palette.
    abstract checkCallback: (bool -> U2<bool, unit>) option with get, set
    /// A command callback that is only triggered when the user is in an editor.
    /// Overrides `callback` and `checkCallback`
    abstract editorCallback: (Editor -> MarkdownView -> obj option) option with get, set
    /// A command callback that is only triggered when the user is in an editor.
    /// Overrides `editorCallback`, `callback` and `checkCallback`
    abstract editorCheckCallback: (bool -> Editor -> MarkdownView -> U2<bool, unit>) option with get, set
    /// Sets the default hotkey. It is recommended for plugins to avoid setting default hotkeys if possible,
    /// to avoid conflicting hotkeys with one that's set by the user, even though customized hotkeys have higher priority.
    abstract hotkeys: ResizeArray<Hotkey> option with get, set

type [<AllowNullLiteral>] Component =
    /// Load this component and its children
    abstract load: unit -> unit
    /// Override this to load your component
    abstract onload: unit -> unit
    /// Unload this component and its children
    abstract unload: unit -> unit
    /// Override this to unload your component
    abstract onunload: unit -> unit
    /// Adds a child component, loading it if this component is loaded
    abstract addChild: ``component``: 'T -> 'T
    /// Removes a child component, unloading it
    abstract removeChild: ``component``: 'T -> 'T
    /// Registers a callback to be called when unloading
    abstract register: cb: (unit -> obj option) -> unit
    /// Registers an event to be detached when unloading
    abstract registerEvent: eventRef: EventRef -> unit
    /// Registers an DOM event to be detached when unloading
    abstract registerDomEvent: el: Window * ``type``: 'K * callback: (HTMLElement -> WindowEventMap -> obj option) -> unit
    /// Registers an DOM event to be detached when unloading
    abstract registerDomEvent: el: Document * ``type``: 'K * callback: (HTMLElement -> DocumentEventMap -> obj option) -> unit
    /// Registers an DOM event to be detached when unloading
    abstract registerDomEvent: el: HTMLElement * ``type``: 'K * callback: (HTMLElement -> HTMLElementEventMap -> obj option) -> unit
    /// Registers an key event to be detached when unloading
    abstract registerScopeEvent: keyHandler: KeymapEventHandler -> unit
    /// Registers an interval (from setInterval) to be cancelled when unloading
    /// Use {@link window.setInterval} instead of {@link setInterval} to avoid TypeScript confusing between NodeJS vs Browser API
    abstract registerInterval: id: float -> float

type [<AllowNullLiteral>] ComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> Component

type [<AllowNullLiteral>] Constructor<'T> =
    interface end

type [<AllowNullLiteral>] ConstructorStatic =
    [<Emit "new $0($1...)">] abstract Create: [<ParamArray>] args: ResizeArray<obj option> -> Constructor<'T>

type [<AllowNullLiteral>] DataAdapter =
    abstract getName: unit -> string
    abstract exists: normalizedPath: string * ?sensitive: bool -> Promise<bool>
    abstract stat: normalizedPath: string -> Promise<Stat option>
    abstract list: normalizedPath: string -> Promise<ListedFiles>
    abstract read: normalizedPath: string -> Promise<string>
    abstract readBinary: normalizedPath: string -> Promise<ArrayBuffer>
    abstract write: normalizedPath: string * data: string * ?options: DataWriteOptions -> Promise<unit>
    abstract writeBinary: normalizedPath: string * data: ArrayBuffer * ?options: DataWriteOptions -> Promise<unit>
    abstract append: normalizedPath: string * data: string * ?options: DataWriteOptions -> Promise<unit>
    abstract getResourcePath: normalizedPath: string -> string
    abstract mkdir: normalizedPath: string -> Promise<unit>
    abstract trashSystem: normalizedPath: string -> Promise<bool>
    abstract trashLocal: normalizedPath: string -> Promise<unit>
    abstract rmdir: normalizedPath: string * recursive: bool -> Promise<unit>
    abstract remove: normalizedPath: string -> Promise<unit>
    abstract rename: normalizedPath: string * normalizedNewPath: string -> Promise<unit>
    abstract copy: normalizedPath: string * normalizedNewPath: string -> Promise<unit>

type [<AllowNullLiteral>] DataWriteOptions =
    abstract ctime: float option with get, set
    abstract mtime: float option with get, set

type [<AllowNullLiteral>] Debouncer<'T> =
    [<Emit "$0($1...)">] abstract Invoke: [<ParamArray>] args: obj -> unit
    abstract cancel: unit -> Debouncer<'T>

type [<AllowNullLiteral>] DropdownComponent =
    inherit ValueComponent<string>
    abstract selectEl: HTMLSelectElement with get, set
    abstract setDisabled: disabled: bool -> DropdownComponent
    abstract addOption: value: string * display: string -> DropdownComponent
    abstract addOptions: options: Record<string, string> -> DropdownComponent
    abstract getValue: unit -> string
    abstract setValue: value: string -> DropdownComponent
    abstract onChange: callback: (string -> obj option) -> DropdownComponent

type [<AllowNullLiteral>] DropdownComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: containerEl: HTMLElement -> DropdownComponent

type [<AllowNullLiteral>] EditableFileView =
    inherit FileView

type [<AllowNullLiteral>] EditableFileViewStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> EditableFileView

/// A common interface that bridges the gap between CodeMirror 5 and CodeMirror 6.
type [<AllowNullLiteral>] Editor =
    abstract getDoc: unit -> Editor
    abstract refresh: unit -> unit
    abstract getValue: unit -> string
    abstract setValue: content: string -> unit
    /// Get the text at line (0-indexed)
    abstract getLine: line: float -> string
    abstract setLine: n: float * text: string -> unit
    /// Gets the number of lines in the document
    abstract lineCount: unit -> float
    abstract lastLine: unit -> float
    abstract getSelection: unit -> string
    abstract somethingSelected: unit -> bool
    abstract getRange: from: EditorPosition * ``to``: EditorPosition -> string
    abstract replaceSelection: replacement: string * ?origin: string -> unit
    abstract replaceRange: replacement: string * from: EditorPosition * ?``to``: EditorPosition * ?origin: string -> unit
    abstract getCursor: ?string: EditorGetCursorString -> EditorPosition
    abstract listSelections: unit -> ResizeArray<EditorSelection>
    abstract setCursor: pos: U2<EditorPosition, float> * ?ch: float -> unit
    abstract setSelection: anchor: EditorPosition * ?head: EditorPosition -> unit
    abstract setSelections: ranges: ResizeArray<EditorSelectionOrCaret> * ?main: float -> unit
    abstract focus: unit -> unit
    abstract blur: unit -> unit
    abstract hasFocus: unit -> bool
    abstract getScrollInfo: unit -> EditorGetScrollInfoReturn
    abstract scrollTo: ?x: float * ?y: float -> unit
    abstract scrollIntoView: range: EditorRange * ?center: bool -> unit
    abstract undo: unit -> unit
    abstract redo: unit -> unit
    abstract exec: command: EditorCommandName -> unit
    abstract transaction: tx: EditorTransaction * ?origin: string -> unit
    abstract wordAt: pos: EditorPosition -> EditorRange option
    abstract posToOffset: pos: EditorPosition -> float
    abstract offsetToPos: offset: float -> EditorPosition
    abstract processLines: read: (float -> string -> 'T option) * write: (float -> string -> 'T option -> U2<EditorChange, unit>) * ?ignoreEmpty: bool -> unit

type [<StringEnum>] [<RequireQualifiedAccess>] EditorGetCursorString =
    | From
    | To
    | Head
    | Anchor

type [<AllowNullLiteral>] EditorGetScrollInfoReturn =
    abstract top: float with get, set
    abstract left: float with get, set

/// A common interface that bridges the gap between CodeMirror 5 and CodeMirror 6.
type [<AllowNullLiteral>] EditorStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> Editor

type [<AllowNullLiteral>] EditorChange =
    inherit EditorRangeOrCaret
    abstract text: string with get, set

type [<StringEnum>] [<RequireQualifiedAccess>] EditorCommandName =
    | GoUp
    | GoDown
    | GoLeft
    | GoRight
    | GoStart
    | GoEnd
    | IndentMore
    | IndentLess
    | NewlineAndIndent
    | SwapLineUp
    | SwapLineDown
    | DeleteLine
    | ToggleFold
    | FoldAll
    | UnfoldAll

type [<AllowNullLiteral>] EditorPosition =
    abstract line: float with get, set
    abstract ch: float with get, set

type [<AllowNullLiteral>] EditorRange =
    abstract from: EditorPosition with get, set
    abstract ``to``: EditorPosition with get, set

type [<AllowNullLiteral>] EditorRangeOrCaret =
    abstract from: EditorPosition with get, set
    abstract ``to``: EditorPosition option with get, set

type [<AllowNullLiteral>] EditorSelection =
    abstract anchor: EditorPosition with get, set
    abstract head: EditorPosition with get, set

type [<AllowNullLiteral>] EditorSelectionOrCaret =
    abstract anchor: EditorPosition with get, set
    abstract head: EditorPosition option with get, set

type [<AllowNullLiteral>] EditorSuggest<'T> =
    inherit PopoverSuggest<'T>
    /// Current suggestion context, containing the result of `onTrigger`.
    /// This will be null any time the EditorSuggest is not supposed to run.
    abstract context: EditorSuggestContext option with get, set
    /// Override this to use a different limit for suggestion items
    abstract limit: float with get, set
    abstract setInstructions: instructions: ResizeArray<Instruction> -> unit
    /// Based on the editor line and cursor position, determine if this EditorSuggest should be triggered at this moment.
    /// Typically, you would run a regular expression on the current line text before the cursor.
    /// Return null to indicate that this editor suggest is not supposed to be triggered.
    /// 
    /// Please be mindful of performance when implementing this function, as it will be triggered very often (on each keypress).
    /// Keep it simple, and return null as early as possible if you determine that it is not the right time.
    abstract onTrigger: cursor: EditorPosition * editor: Editor * file: TFile -> EditorSuggestTriggerInfo option
    /// Generate suggestion items based on this context. Can be async, but preferably sync.
    /// When generating async suggestions, you should pass the context along.
    abstract getSuggestions: context: EditorSuggestContext -> U2<ResizeArray<'T>, Promise<ResizeArray<'T>>>

type [<AllowNullLiteral>] EditorSuggestStatic =
    [<Emit "new $0($1...)">] abstract Create: app: App -> EditorSuggest<'T>

type [<AllowNullLiteral>] EditorSuggestContext =
    inherit EditorSuggestTriggerInfo
    abstract editor: Editor with get, set
    abstract file: TFile with get, set

type [<AllowNullLiteral>] EditorSuggestTriggerInfo =
    /// The start position of the triggering text. This is used to position the popover.
    abstract start: EditorPosition with get, set
    /// The end position of the triggering text. This is used to position the popover.
    abstract ``end``: EditorPosition with get, set
    /// They query string (usually the text between start and end) that will be used to generate the suggestion content.
    abstract query: string with get, set

type [<AllowNullLiteral>] EditorTransaction =
    abstract replaceSelection: string option with get, set
    abstract changes: ResizeArray<EditorChange> option with get, set
    /// Multiple selections, overrides `selection`.
    abstract selections: ResizeArray<EditorRangeOrCaret> option with get, set
    abstract selection: EditorRangeOrCaret option with get, set

type [<AllowNullLiteral>] EmbedCache =
    inherit ReferenceCache

type [<AllowNullLiteral>] EventRef =
    interface end

type [<AllowNullLiteral>] Events =
    abstract on: name: string * callback: (obj option -> obj option) * ?ctx: obj -> EventRef
    abstract off: name: string * callback: (obj option -> obj option) -> unit
    abstract offref: ref: EventRef -> unit
    abstract trigger: name: string * [<ParamArray>] data: ResizeArray<obj option> -> unit
    abstract tryTrigger: evt: EventRef * args: ResizeArray<obj option> -> unit

type [<AllowNullLiteral>] EventsStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> Events

type [<AllowNullLiteral>] ExtraButtonComponent =
    inherit BaseComponent
    abstract extraSettingsEl: HTMLElement with get, set
    abstract setDisabled: disabled: bool -> ExtraButtonComponent
    abstract setTooltip: tooltip: string -> ExtraButtonComponent
    abstract setIcon: icon: string -> ExtraButtonComponent
    abstract onClick: callback: (unit -> obj option) -> ExtraButtonComponent

type [<AllowNullLiteral>] ExtraButtonComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: containerEl: HTMLElement -> ExtraButtonComponent

/// Manage the creation, deletion and renaming of files from the UI.
type [<AllowNullLiteral>] FileManager =
    /// <summary>Gets the folder that new files should be saved to, given the user's preferences.</summary>
    /// <param name="sourcePath">- The path to the current open/focused file,
    /// used when the user wants new files to be created "in the same folder".
    /// Use an empty string if there is no active file.</param>
    abstract getNewFileParent: sourcePath: string -> TFolder
    /// <summary>Rename or move a file safely, and update all links to it depending on the user's preferences.</summary>
    /// <param name="file">- the file to rename</param>
    /// <param name="newPath">- the new path for the file</param>
    abstract renameFile: file: TAbstractFile * newPath: string -> Promise<unit>
    /// <summary>Generate a markdown link based on the user's preferences.</summary>
    /// <param name="file">- the file to link to.</param>
    /// <param name="sourcePath">- where the link is stored in, used to compute relative links.</param>
    /// <param name="subpath">- A subpath, starting with `#`, used for linking to headings or blocks.</param>
    /// <param name="alias">- The display text if it's to be different than the file name. Pass empty string to use file name.</param>
    abstract generateMarkdownLink: file: TFile * sourcePath: string * ?subpath: string * ?alias: string -> string

/// Manage the creation, deletion and renaming of files from the UI.
type [<AllowNullLiteral>] FileManagerStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> FileManager

type [<AllowNullLiteral>] FileStats =
    abstract ctime: float with get, set
    abstract mtime: float with get, set
    abstract size: float with get, set

type [<AllowNullLiteral>] FileSystemAdapter =
    inherit DataAdapter
    abstract getName: unit -> string
    abstract getBasePath: unit -> string
    abstract mkdir: normalizedPath: string -> Promise<unit>
    abstract trashSystem: normalizedPath: string -> Promise<bool>
    abstract trashLocal: normalizedPath: string -> Promise<unit>
    abstract rmdir: normalizedPath: string * recursive: bool -> Promise<unit>
    abstract read: normalizedPath: string -> Promise<string>
    abstract readBinary: normalizedPath: string -> Promise<ArrayBuffer>
    abstract write: normalizedPath: string * data: string * ?options: DataWriteOptions -> Promise<unit>
    abstract writeBinary: normalizedPath: string * data: ArrayBuffer * ?options: DataWriteOptions -> Promise<unit>
    abstract append: normalizedPath: string * data: string * ?options: DataWriteOptions -> Promise<unit>
    abstract getResourcePath: normalizedPath: string -> string
    abstract remove: normalizedPath: string -> Promise<unit>
    abstract rename: normalizedPath: string * normalizedNewPath: string -> Promise<unit>
    abstract copy: normalizedPath: string * normalizedNewPath: string -> Promise<unit>
    abstract exists: normalizedPath: string * ?sensitive: bool -> Promise<bool>
    abstract stat: normalizedPath: string -> Promise<Stat option>
    abstract list: normalizedPath: string -> Promise<ListedFiles>
    abstract getFullPath: normalizedPath: string -> string

type [<AllowNullLiteral>] FileSystemAdapterStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> FileSystemAdapter
    abstract readLocalFile: path: string -> Promise<ArrayBuffer>
    abstract mkdir: path: string -> Promise<unit>

type [<AllowNullLiteral>] FileView =
    inherit ItemView
    abstract allowNoFile: bool with get, set
    abstract file: TFile with get, set
    abstract getDisplayText: unit -> string
    /// Override this to load your component
    abstract onload: unit -> unit
    abstract getState: unit -> obj option
    abstract setState: state: obj option * result: ViewStateResult -> Promise<unit>
    abstract onLoadFile: file: TFile -> Promise<unit>
    abstract onUnloadFile: file: TFile -> Promise<unit>
    abstract onMoreOptionsMenu: menu: Menu -> unit
    abstract onHeaderMenu: menu: Menu -> unit
    abstract canAcceptExtension: extension: string -> bool

type [<AllowNullLiteral>] FileViewStatic =
    [<Emit "new $0($1...)">] abstract Create: leaf: WorkspaceLeaf -> FileView

type [<AllowNullLiteral>] FrontMatterCache =
    inherit CacheItem
    [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> obj option with get, set

type [<AllowNullLiteral>] FuzzyMatch<'T> =
    abstract item: 'T with get, set
    abstract ``match``: SearchResult with get, set

type [<AllowNullLiteral>] FuzzySuggestModal<'T> =
    inherit SuggestModal<FuzzyMatch<'T>>
    abstract getSuggestions: query: string -> ResizeArray<FuzzyMatch<'T>>
    /// Render the suggestion item into DOM.
    abstract renderSuggestion: item: FuzzyMatch<'T> * el: HTMLElement -> unit
    abstract onChooseSuggestion: item: FuzzyMatch<'T> * evt: U2<MouseEvent, KeyboardEvent> -> unit
    abstract getItems: unit -> ResizeArray<'T>
    abstract getItemText: item: 'T -> string
    abstract onChooseItem: item: 'T * evt: U2<MouseEvent, KeyboardEvent> -> unit

type [<AllowNullLiteral>] FuzzySuggestModalStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> FuzzySuggestModal<'T>

type [<AllowNullLiteral>] HeadingCache =
    inherit CacheItem
    abstract heading: string with get, set
    abstract level: float with get, set

type [<AllowNullLiteral>] HeadingSubpathResult =
    inherit SubpathResult
    abstract ``type``: string with get, set
    abstract current: HeadingCache with get, set
    abstract next: HeadingCache with get, set

type [<AllowNullLiteral>] Hotkey =
    abstract modifiers: ResizeArray<Modifier> with get, set
    abstract key: string with get, set

type [<AllowNullLiteral>] HoverParent =
    abstract hoverPopover: HoverPopover option with get, set

type [<AllowNullLiteral>] HoverPopover =
    inherit Component
    abstract state: PopoverState with get, set

type [<AllowNullLiteral>] HoverPopoverStatic =
    [<Emit "new $0($1...)">] abstract Create: parent: HoverParent * targetEl: HTMLElement option * ?waitTime: float -> HoverPopover

type [<AllowNullLiteral>] Instruction =
    abstract command: string with get, set
    abstract purpose: string with get, set

type [<AllowNullLiteral>] ISuggestOwner<'T> =
    /// Render the suggestion item into DOM.
    abstract renderSuggestion: value: 'T * el: HTMLElement -> unit
    /// Called when the user makes a selection.
    abstract selectSuggestion: value: 'T * evt: U2<MouseEvent, KeyboardEvent> -> unit

type [<AllowNullLiteral>] ItemView =
    inherit View
    abstract contentEl: HTMLElement with get, set
    abstract onMoreOptionsMenu: menu: Menu -> unit
    abstract addAction: icon: string * title: string * callback: (MouseEvent -> obj option) * ?size: float -> HTMLElement
    abstract onHeaderMenu: menu: Menu -> unit

type [<AllowNullLiteral>] ItemViewStatic =
    [<Emit "new $0($1...)">] abstract Create: leaf: WorkspaceLeaf -> ItemView

type [<AllowNullLiteral>] Keymap =
    abstract pushScope: scope: Scope -> unit
    abstract popScope: scope: Scope -> unit

type [<AllowNullLiteral>] KeymapStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> Keymap
    /// Checks whether the modifier key is pressed during this event
    abstract isModifier: evt: U3<MouseEvent, TouchEvent, KeyboardEvent> * modifier: Modifier -> bool
    /// Returns true if the modifier key Cmd/Ctrl is pressed OR if this is a middle-click MouseEvent.
    abstract isModEvent: ?evt: UserEvent -> bool

type [<AllowNullLiteral>] KeymapContext =
    inherit KeymapInfo
    abstract vkey: string with get, set

type [<AllowNullLiteral>] KeymapEventHandler =
    inherit KeymapInfo
    abstract scope: Scope with get, set

type [<AllowNullLiteral>] KeymapEventListener =
    [<Emit "$0($1...)">] abstract Invoke: evt: KeyboardEvent * ctx: KeymapContext -> U2<bool, unit>

type [<AllowNullLiteral>] KeymapInfo =
    abstract modifiers: string option with get, set
    abstract key: string option with get, set

type [<AllowNullLiteral>] LinkCache =
    inherit ReferenceCache

type [<AllowNullLiteral>] ListedFiles =
    abstract files: ResizeArray<string> with get, set
    abstract folders: ResizeArray<string> with get, set

type [<AllowNullLiteral>] ListItemCache =
    inherit CacheItem
    /// The block ID of this list item, if defined.
    abstract id: string option with get, set
    /// A single character indicating the checked status of a task.
    /// The space character `' '` is interpreted as an incomplete task.
    /// An other character is interpreted as completed task.
    /// `undefined` if this item isn't a task.
    abstract task: string option with get, set
    /// Line number of the parent list item (position.start.line).
    /// If this item has no parent (e.g. it's a root level list),
    /// then this value is the negative of the line number of the first list item (start of the list).
    /// 
    /// Can be used to deduce which list items belongs to the same group (item1.parent === item2.parent).
    /// Can be used to reconstruct hierarchy information (parentItem.position.start.line === childItem.parent).
    abstract parent: float with get, set

type [<AllowNullLiteral>] Loc =
    abstract line: float with get, set
    abstract col: float with get, set
    abstract offset: float with get, set

/// This is the editor for Obsidian Mobile as well as the upcoming WYSIWYG editor.
type [<AllowNullLiteral>] MarkdownEditView =
    inherit MarkdownSubView
    inherit HoverParent
    abstract hoverPopover: HoverPopover with get, set
    abstract clear: unit -> unit
    abstract get: unit -> string
    abstract set: data: string * clear: bool -> unit
    abstract getSelection: unit -> string
    abstract getScroll: unit -> float
    abstract applyScroll: scroll: float -> unit

/// This is the editor for Obsidian Mobile as well as the upcoming WYSIWYG editor.
type [<AllowNullLiteral>] MarkdownEditViewStatic =
    [<Emit "new $0($1...)">] abstract Create: view: MarkdownView -> MarkdownEditView

/// A post processor receives an element which is a section of the preview.
/// 
/// Post processors can mutate the DOM to render various things, such as mermaid graphs, latex equations, or custom controls.
/// 
/// If your post processor requires lifecycle management, for example, to clear an interval, kill a subprocess, etc when this element is
/// removed from the app, look into {@link MarkdownPostProcessorContext#addChild}
type [<AllowNullLiteral>] MarkdownPostProcessor =
    /// The processor function itself.
    [<Emit "$0($1...)">] abstract Invoke: el: HTMLElement * ctx: MarkdownPostProcessorContext -> U2<Promise<obj option>, unit>
    /// An optional integer sort order. Defaults to 0. Lower number runs before higher numbers.
    abstract sortOrder: float option with get, set

type [<AllowNullLiteral>] MarkdownPostProcessorContext =
    abstract docId: string with get, set
    abstract sourcePath: string with get, set
    abstract frontmatter: obj option option with get, set
    /// Adds a child component that will have its lifecycle managed by the renderer.
    /// 
    /// Use this to add a dependent child to the renderer such that if the containerEl
    /// of the child is ever removed, the component's unload will be called.
    abstract addChild: child: MarkdownRenderChild -> unit
    /// Gets the section information of this element at this point in time.
    /// Only call this function right before you need this information to get the most up-to-date version.
    /// This function may also return null in many circumstances; if you use it, you must be prepared to deal with nulls.
    abstract getSectionInfo: el: HTMLElement -> MarkdownSectionInformation option

type [<AllowNullLiteral>] MarkdownPreviewEvents =
    inherit Component

type [<AllowNullLiteral>] MarkdownPreviewRenderer =
    interface end

type [<AllowNullLiteral>] MarkdownPreviewRendererStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> MarkdownPreviewRenderer
    abstract registerPostProcessor: postProcessor: MarkdownPostProcessor * ?sortOrder: float -> unit
    abstract unregisterPostProcessor: postProcessor: MarkdownPostProcessor -> unit
    abstract createCodeBlockPostProcessor: language: string * handler: (string -> HTMLElement -> MarkdownPostProcessorContext -> U2<Promise<obj option>, unit>) -> (HTMLElement -> MarkdownPostProcessorContext -> unit)

type [<AllowNullLiteral>] MarkdownPreviewView =
    inherit MarkdownRenderer
    inherit MarkdownSubView
    inherit MarkdownPreviewEvents
    abstract containerEl: HTMLElement with get, set
    abstract get: unit -> string
    abstract set: data: string * clear: bool -> unit
    abstract clear: unit -> unit
    abstract rerender: ?full: bool -> unit
    abstract getScroll: unit -> float
    abstract applyScroll: scroll: float -> unit

type [<AllowNullLiteral>] MarkdownPreviewViewStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> MarkdownPreviewView

type [<AllowNullLiteral>] MarkdownRenderChild =
    inherit Component
    abstract containerEl: HTMLElement with get, set

type [<AllowNullLiteral>] MarkdownRenderChildStatic =
    /// <param name="containerEl">- This HTMLElement will be used to test whether this component is still alive.
    /// It should be a child of the markdown preview sections, and when it's no longer attached
    /// (for example, when it is replaced with a new version because the user edited the markdown source code),
    /// this component will be unloaded.</param>
    [<Emit "new $0($1...)">] abstract Create: containerEl: HTMLElement -> MarkdownRenderChild

type [<AllowNullLiteral>] MarkdownRenderer =
    inherit MarkdownRenderChild
    inherit MarkdownPreviewEvents
    inherit HoverParent
    abstract hoverPopover: HoverPopover with get, set

type [<AllowNullLiteral>] MarkdownRendererStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> MarkdownRenderer
    /// <summary>Renders markdown string to an HTML element.</summary>
    /// <param name="markdown">- The markdown source code</param>
    /// <param name="el">- The element to append to</param>
    /// <param name="sourcePath">- The normalized path of this markdown file, used to resolve relative internal links</param>
    /// <param name="component">- A parent component to manage the lifecycle of the rendered child components, if any</param>
    abstract renderMarkdown: markdown: string * el: HTMLElement * sourcePath: string * ``component``: Component -> Promise<unit>

type [<AllowNullLiteral>] MarkdownSectionInformation =
    abstract text: string with get, set
    abstract lineStart: float with get, set
    abstract lineEnd: float with get, set

type [<AllowNullLiteral>] MarkdownSourceView =
    inherit MarkdownSubView
    inherit HoverParent
    abstract cmEditor: CodeMirror.Editor with get, set
    abstract hoverPopover: HoverPopover with get, set
    abstract clear: unit -> unit
    abstract get: unit -> string
    abstract set: data: string * clear: bool -> unit
    abstract getSelection: unit -> string
    abstract getScroll: unit -> float
    abstract applyScroll: scroll: float -> unit

type [<AllowNullLiteral>] MarkdownSourceViewStatic =
    [<Emit "new $0($1...)">] abstract Create: view: MarkdownView -> MarkdownSourceView

type [<AllowNullLiteral>] MarkdownSubView =
    abstract getScroll: unit -> float
    abstract applyScroll: scroll: float -> unit
    abstract get: unit -> string
    abstract set: data: string * clear: bool -> unit

type [<AllowNullLiteral>] MarkdownView =
    inherit TextFileView
    abstract editor: Editor with get, set
    abstract previewMode: MarkdownPreviewView with get, set
    abstract currentMode: MarkdownSubView with get, set
    abstract getViewType: unit -> string
    abstract getMode: unit -> MarkdownViewModeType
    /// Gets the data from the editor. This will be called to save the editor contents to the file.
    abstract getViewData: unit -> string
    /// Clear the editor. This is usually called when we're about to open a completely
    /// different file, so it's best to clear any editor states like undo-redo history,
    /// and any caches/indexes associated with the previous file contents.
    abstract clear: unit -> unit
    /// Set the data to the editor. This is used to load the file contents.
    /// 
    /// If clear is set, then it means we're opening a completely different file.
    /// In that case, you should call clear(), or implement a slightly more efficient
    /// clearing mechanism given the new data to be set.
    abstract setViewData: data: string * clear: bool -> unit
    abstract showSearch: ?replace: bool -> unit

type [<AllowNullLiteral>] MarkdownViewStatic =
    [<Emit "new $0($1...)">] abstract Create: leaf: WorkspaceLeaf -> MarkdownView

type [<StringEnum>] [<RequireQualifiedAccess>] MarkdownViewModeType =
    | Source
    | Preview

type [<AllowNullLiteral>] Menu =
    inherit Component
    abstract setNoIcon: unit -> Menu
    abstract addItem: cb: (MenuItem -> obj option) -> Menu
    abstract addSeparator: unit -> Menu
    abstract showAtMouseEvent: evt: MouseEvent -> Menu
    abstract showAtPosition: position: Point -> Menu
    abstract hide: unit -> Menu
    abstract onHide: callback: (unit -> obj option) -> unit

type [<AllowNullLiteral>] MenuStatic =
    [<Emit "new $0($1...)">] abstract Create: app: App -> Menu

type [<AllowNullLiteral>] MenuItem =
    abstract setTitle: title: U2<string, DocumentFragment> -> MenuItem
    abstract setIcon: icon: string option * ?size: float -> MenuItem
    abstract setActive: active: bool -> MenuItem
    abstract setDisabled: disabled: bool -> MenuItem
    abstract setIsLabel: isLabel: bool -> MenuItem
    abstract onClick: callback: (U2<MouseEvent, KeyboardEvent> -> obj option) -> MenuItem

type [<AllowNullLiteral>] MenuItemStatic =
    [<Emit "new $0($1...)">] abstract Create: menu: Menu -> MenuItem

/// Linktext is any internal link that is composed of a path and a subpath, such as "My note#Heading"
/// Linkpath (or path) is the path part of a linktext
/// Subpath is the heading/block ID part of a linktext.
type [<AllowNullLiteral>] MetadataCache =
    inherit Events
    /// Get the best match for a linkpath.
    abstract getFirstLinkpathDest: linkpath: string * sourcePath: string -> TFile option
    abstract getFileCache: file: TFile -> CachedMetadata option
    abstract getCache: path: string -> CachedMetadata
    /// Generates a linktext for a file.
    /// 
    /// If file name is unique, use the filename.
    /// If not unique, use full path.
    abstract fileToLinktext: file: TFile * sourcePath: string * ?omitMdExtension: bool -> string
    /// Contains all resolved links. This object maps each source file's path to an object of destination file paths with the link count.
    /// Source and destination paths are all vault absolute paths that comes from `TFile.path` and can be used with `Vault.getAbstractFileByPath(path)`.
    abstract resolvedLinks: Record<string, Record<string, float>> with get, set
    /// Contains all unresolved links. This object maps each source file to an object of unknown destinations with count.
    /// Source paths are all vault absolute paths, similar to `resolvedLinks`.
    abstract unresolvedLinks: Record<string, Record<string, float>> with get, set
    /// Called when a file has been indexed, and its (updated) cache is now available.
    [<Emit "$0.on('changed',$1,$2)">] abstract on_changed: callback: (TFile -> string -> CachedMetadata -> obj option) * ?ctx: obj -> EventRef
    /// Called when a file has been deleted. A best-effort previous version of the cached metadata is presented,
    /// but it could be null in case the file was not successfully cached previously.
    [<Emit "$0.on('deleted',$1,$2)">] abstract on_deleted: callback: (TFile -> CachedMetadata option -> obj option) * ?ctx: obj -> EventRef
    /// Called when a file has been resolved for `resolvedLinks` and `unresolvedLinks`.
    /// This happens sometimes after a file has been indexed.
    [<Emit "$0.on('resolve',$1,$2)">] abstract on_resolve: callback: (TFile -> obj option) * ?ctx: obj -> EventRef
    /// Called when all files has been resolved. This will be fired each time files get modified after the initial load.
    [<Emit "$0.on('resolved',$1,$2)">] abstract on_resolved: callback: (unit -> obj option) * ?ctx: obj -> EventRef

/// Linktext is any internal link that is composed of a path and a subpath, such as "My note#Heading"
/// Linkpath (or path) is the path part of a linktext
/// Subpath is the heading/block ID part of a linktext.
type [<AllowNullLiteral>] MetadataCacheStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> MetadataCache

type [<AllowNullLiteral>] Modal =
    inherit CloseableComponent
    abstract app: App with get, set
    abstract scope: Scope with get, set
    abstract containerEl: HTMLElement with get, set
    abstract modalEl: HTMLElement with get, set
    abstract titleEl: HTMLElement with get, set
    abstract contentEl: HTMLElement with get, set
    abstract shouldRestoreSelection: bool with get, set
    abstract ``open``: unit -> unit
    abstract close: unit -> unit
    abstract onOpen: unit -> unit
    abstract onClose: unit -> unit

type [<AllowNullLiteral>] ModalStatic =
    [<Emit "new $0($1...)">] abstract Create: app: App -> Modal

type [<StringEnum>] [<RequireQualifiedAccess>] Modifier =
    | [<CompiledName "Mod">] Mod
    | [<CompiledName "Ctrl">] Ctrl
    | [<CompiledName "Meta">] Meta
    | [<CompiledName "Shift">] Shift
    | [<CompiledName "Alt">] Alt

type [<AllowNullLiteral>] MomentFormatComponent =
    inherit TextComponent
    abstract sampleEl: HTMLElement with get, set
    /// Sets the default format when input is cleared. Also used for placeholder.
    abstract setDefaultFormat: defaultFormat: string -> MomentFormatComponent
    abstract setSampleEl: sampleEl: HTMLElement -> MomentFormatComponent
    abstract setValue: value: string -> MomentFormatComponent
    abstract onChanged: unit -> unit
    abstract updateSample: unit -> unit

type [<AllowNullLiteral>] MomentFormatComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> MomentFormatComponent

type [<AllowNullLiteral>] Notice =
    /// Change the message of this notice.
    abstract setMessage: message: U2<string, DocumentFragment> -> Notice
    abstract hide: unit -> unit

type [<AllowNullLiteral>] NoticeStatic =
    [<Emit "new $0($1...)">] abstract Create: message: U2<string, DocumentFragment> * ?timeout: float -> Notice

type [<AllowNullLiteral>] ObsidianProtocolData =
    abstract action: string with get, set
    [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> string with get, set

type [<AllowNullLiteral>] ObsidianProtocolHandler =
    [<Emit "$0($1...)">] abstract Invoke: ``params``: ObsidianProtocolData -> obj option

type [<AllowNullLiteral>] OpenViewState =
    interface end

type [<AllowNullLiteral>] Plugin_2 =
    inherit Component
    abstract app: App with get, set
    abstract manifest: PluginManifest with get, set
    /// <summary>Adds a ribbon icon to the left bar.</summary>
    /// <param name="icon">- The icon name to be used. See {@link addIcon}</param>
    /// <param name="title">- The title to be displayed in the tooltip.</param>
    /// <param name="callback">- The `click` callback.</param>
    abstract addRibbonIcon: icon: string * title: string * callback: (MouseEvent -> obj option) -> HTMLElement
    abstract addStatusBarItem: unit -> HTMLElement
    /// Register a command globally. The command id and name will be automatically prefixed with this plugin's id and name.
    abstract addCommand: command: Command -> Command
    abstract addSettingTab: settingTab: PluginSettingTab -> unit
    abstract registerView: ``type``: string * viewCreator: ViewCreator -> unit
    abstract registerExtensions: extensions: ResizeArray<string> * viewType: string -> unit
    abstract registerMarkdownPostProcessor: postProcessor: MarkdownPostProcessor * ?sortOrder: float -> MarkdownPostProcessor
    /// Register a special post processor that handles fenced code given a language and a handler.
    /// This special post processor takes care of removing the <pre><code> and create a <div> that
    /// will be passed to your handler, and is expected to be filled with your custom elements.
    abstract registerMarkdownCodeBlockProcessor: language: string * handler: (string -> HTMLElement -> MarkdownPostProcessorContext -> U2<Promise<obj option>, unit>) * ?sortOrder: float -> MarkdownPostProcessor
    /// Runs callback on all currently loaded instances of CodeMirror,
    /// then registers the callback for all future CodeMirror instances.
    abstract registerCodeMirror: callback: (CodeMirror.Editor -> obj option) -> unit
    /// <summary>Registers a CodeMirror 6 extension.
    /// To reconfigure cm6 extensions for your plugin on the fly, you can pass an array here and dynamically
    /// modify it. Once this array is modified, call `Workspace.updateOptions()` to have the changes applied.</summary>
    /// <param name="extension">- must be a CodeMirror 6 `Extension`, or an array of Extensions.</param>
    abstract registerEditorExtension: extension: Extension -> unit
    /// <summary>Register a handler for obsidian:// URLs.</summary>
    /// <param name="action">- the action string. For example, "open" corresponds to `obsidian://open`.</param>
    /// <param name="handler">- the callback to trigger. You will be passed the key-value pair that is decoded from the query.
    ///   For example, `obsidian://open?key=value` would generate `{"action": "open", "key": "value"}`.</param>
    abstract registerObsidianProtocolHandler: action: string * handler: ObsidianProtocolHandler -> unit
    /// Register an EditorSuggest which can provide live suggestions while the user is typing.
    abstract registerEditorSuggest: editorSuggest: EditorSuggest<obj option> -> unit
    abstract loadData: unit -> Promise<obj option>
    abstract saveData: data: obj option -> Promise<unit>

type [<AllowNullLiteral>] Plugin_2Static =
    [<Emit "new $0($1...)">] abstract Create: app: App * manifest: PluginManifest -> Plugin_2

type [<AllowNullLiteral>] PluginManifest =
    abstract dir: string option with get, set
    abstract id: string with get, set
    abstract name: string with get, set
    abstract author: string with get, set
    abstract version: string with get, set
    abstract minAppVersion: string with get, set
    abstract description: string with get, set
    abstract authorUrl: string option with get, set
    abstract isDesktopOnly: bool option with get, set

type [<AllowNullLiteral>] PluginSettingTab =
    inherit SettingTab

type [<AllowNullLiteral>] PluginSettingTabStatic =
    [<Emit "new $0($1...)">] abstract Create: app: App * plugin: Plugin_2 -> PluginSettingTab

type [<AllowNullLiteral>] Point =
    abstract x: float with get, set
    abstract y: float with get, set

type [<RequireQualifiedAccess>] PopoverState =

type [<AllowNullLiteral>] PopoverSuggest<'T> =
    inherit ISuggestOwner<'T>
    inherit CloseableComponent
    abstract ``open``: unit -> unit
    abstract close: unit -> unit
    /// Render the suggestion item into DOM.
    abstract renderSuggestion: value: 'T * el: HTMLElement -> unit
    /// Called when the user makes a selection.
    abstract selectSuggestion: value: 'T * evt: U2<MouseEvent, KeyboardEvent> -> unit

type [<AllowNullLiteral>] PopoverSuggestStatic =
    [<Emit "new $0($1...)">] abstract Create: app: App * ?scope: Scope -> PopoverSuggest<'T>

type [<AllowNullLiteral>] Pos =
    abstract start: Loc with get, set
    abstract ``end``: Loc with get, set

type [<AllowNullLiteral>] PreparedQuery =
    abstract query: string with get, set
    abstract tokens: ResizeArray<string> with get, set
    abstract fuzzy: ResizeArray<string> with get, set

type [<AllowNullLiteral>] Rect_2 =
    abstract x: float with get, set
    abstract y: float with get, set
    abstract w: float with get, set
    abstract h: float with get, set

type [<AllowNullLiteral>] ReferenceCache =
    inherit CacheItem
    abstract link: string with get, set
    abstract original: string with get, set
    /// if title is different than link text, in the case of [[page name|display name]]
    abstract displayText: string option with get, set

type [<AllowNullLiteral>] RequestUrlParam =
    abstract url: string with get, set
    abstract ``method``: string option with get, set
    abstract contentType: string option with get, set
    abstract body: U2<string, ArrayBuffer> option with get, set
    abstract headers: Record<string, string> option with get, set

type [<AllowNullLiteral>] RequestUrlResponse =
    abstract status: float with get, set
    abstract headers: Record<string, string> with get, set
    abstract arrayBuffer: ArrayBuffer with get, set
    abstract json: obj option with get, set
    abstract text: string with get, set

type [<AllowNullLiteral>] Scope =
    /// <param name="modifiers">- `Mod`, `Ctrl`, `Meta`, `Shift`, or `Alt`. `Mod` translates to `Meta` on macOS and `Ctrl` otherwise.</param>
    /// <param name="key">- Keycode from https://developer.mozilla.org/en-US/docs/Web/API/KeyboardEvent/key/Key_Values</param>
    /// <param name="func">- the callback</param>
    abstract register: modifiers: ResizeArray<Modifier> * key: string option * func: KeymapEventListener -> KeymapEventHandler
    abstract unregister: handler: KeymapEventHandler -> unit

type [<AllowNullLiteral>] ScopeStatic =
    [<Emit "new $0($1...)">] abstract Create: ?parent: Scope -> Scope

type [<AllowNullLiteral>] SearchComponent =
    inherit AbstractTextComponent<HTMLInputElement>
    abstract clearButtonEl: HTMLElement with get, set
    abstract onChanged: unit -> unit

type [<AllowNullLiteral>] SearchComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: containerEl: HTMLElement -> SearchComponent

type SearchMatches =
    ResizeArray<SearchMatchPart>

type SearchMatchPart =
    float * float

type [<AllowNullLiteral>] SearchResult =
    abstract score: float with get, set
    abstract matches: SearchMatches with get, set

type [<AllowNullLiteral>] SearchResultContainer =
    abstract ``match``: SearchResult with get, set

type [<AllowNullLiteral>] SectionCache =
    inherit CacheItem
    /// The block ID of this section, if defined.
    abstract id: string option with get, set
    /// The type string generated by the parser.
    abstract ``type``: string with get, set

type [<AllowNullLiteral>] Setting =
    abstract settingEl: HTMLElement with get, set
    abstract infoEl: HTMLElement with get, set
    abstract nameEl: HTMLElement with get, set
    abstract descEl: HTMLElement with get, set
    abstract controlEl: HTMLElement with get, set
    abstract components: ResizeArray<BaseComponent> with get, set
    abstract setName: name: U2<string, DocumentFragment> -> Setting
    abstract setDesc: desc: U2<string, DocumentFragment> -> Setting
    abstract setClass: cls: string -> Setting
    abstract setTooltip: tooltip: string -> Setting
    abstract setHeading: unit -> Setting
    abstract setDisabled: disabled: bool -> Setting
    abstract addButton: cb: (ButtonComponent -> obj option) -> Setting
    abstract addExtraButton: cb: (ExtraButtonComponent -> obj option) -> Setting
    abstract addToggle: cb: (ToggleComponent -> obj option) -> Setting
    abstract addText: cb: (TextComponent -> obj option) -> Setting
    abstract addSearch: cb: (SearchComponent -> obj option) -> Setting
    abstract addTextArea: cb: (TextAreaComponent -> obj option) -> Setting
    abstract addMomentFormat: cb: (MomentFormatComponent -> obj option) -> Setting
    abstract addDropdown: cb: (DropdownComponent -> obj option) -> Setting
    abstract addSlider: cb: (SliderComponent -> obj option) -> Setting
    /// Facilitates chaining
    abstract ``then``: cb: (Setting -> obj option) -> Setting
    abstract clear: unit -> Setting

type [<AllowNullLiteral>] SettingStatic =
    [<Emit "new $0($1...)">] abstract Create: containerEl: HTMLElement -> Setting

type [<AllowNullLiteral>] SettingTab =
    abstract app: App with get, set
    abstract containerEl: HTMLElement with get, set
    abstract display: unit -> obj option
    abstract hide: unit -> obj option

type [<AllowNullLiteral>] SettingTabStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> SettingTab

type [<AllowNullLiteral>] SliderComponent =
    inherit ValueComponent<float>
    abstract sliderEl: HTMLInputElement with get, set
    abstract setDisabled: disabled: bool -> SliderComponent
    abstract setLimits: min: float * max: float * step: U2<float, string> -> SliderComponent
    abstract getValue: unit -> float
    abstract setValue: value: float -> SliderComponent
    abstract getValuePretty: unit -> string
    abstract setDynamicTooltip: unit -> SliderComponent
    abstract showTooltip: unit -> unit
    abstract onChange: callback: (float -> obj option) -> SliderComponent

type [<AllowNullLiteral>] SliderComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: containerEl: HTMLElement -> SliderComponent

type [<StringEnum>] [<RequireQualifiedAccess>] SplitDirection =
    | Vertical
    | Horizontal

type [<AllowNullLiteral>] Stat =
    abstract ``type``: StatType with get, set
    abstract ctime: float with get, set
    abstract mtime: float with get, set
    abstract size: float with get, set

type [<AllowNullLiteral>] SubpathResult =
    abstract start: Loc with get, set
    abstract ``end``: Loc option with get, set

type [<AllowNullLiteral>] SuggestModal<'T> =
    inherit Modal
    inherit ISuggestOwner<'T>
    abstract limit: float with get, set
    abstract emptyStateText: string with get, set
    abstract inputEl: HTMLInputElement with get, set
    abstract resultContainerEl: HTMLElement with get, set
    abstract setPlaceholder: placeholder: string -> unit
    abstract setInstructions: instructions: ResizeArray<Instruction> -> unit
    abstract onNoSuggestion: unit -> unit
    /// Called when the user makes a selection.
    abstract selectSuggestion: value: 'T * evt: U2<MouseEvent, KeyboardEvent> -> unit
    abstract getSuggestions: query: string -> U2<ResizeArray<'T>, Promise<ResizeArray<'T>>>
    /// Render the suggestion item into DOM.
    abstract renderSuggestion: value: 'T * el: HTMLElement -> obj option
    abstract onChooseSuggestion: item: 'T * evt: U2<MouseEvent, KeyboardEvent> -> obj option

type [<AllowNullLiteral>] SuggestModalStatic =
    [<Emit "new $0($1...)">] abstract Create: app: App -> SuggestModal<'T>

/// This can be either a `TFile` or a `TFolder`.
type [<AllowNullLiteral>] TAbstractFile =
    abstract vault: Vault with get, set
    abstract path: string with get, set
    abstract name: string with get, set
    abstract parent: TFolder with get, set

/// This can be either a `TFile` or a `TFolder`.
type [<AllowNullLiteral>] TAbstractFileStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> TAbstractFile

type [<AllowNullLiteral>] TagCache =
    inherit CacheItem
    abstract tag: string with get, set

type [<AllowNullLiteral>] Tasks =
    abstract add: callback: (unit -> Promise<obj option>) -> unit
    abstract addPromise: promise: Promise<obj option> -> unit
    abstract isEmpty: unit -> bool
    abstract promise: unit -> Promise<obj option>

type [<AllowNullLiteral>] TasksStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> Tasks

type [<AllowNullLiteral>] TextAreaComponent =
    inherit AbstractTextComponent<HTMLTextAreaElement>

type [<AllowNullLiteral>] TextAreaComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: containerEl: HTMLElement -> TextAreaComponent

type [<AllowNullLiteral>] TextComponent =
    inherit AbstractTextComponent<HTMLInputElement>

type [<AllowNullLiteral>] TextComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: containerEl: HTMLElement -> TextComponent

/// This class implements a plaintext-based editable file view, which can be loaded and saved given an editor.
/// 
/// Note that by default, this view only saves when it's closing. To implement auto-save, your editor should
/// call `this.requestSave()` when the content is changed.
type [<AllowNullLiteral>] TextFileView =
    inherit EditableFileView
    /// In memory data
    abstract data: string with get, set
    /// Debounced save in 2 seconds from now
    abstract requestSave: (unit -> unit) with get, set
    abstract onUnloadFile: file: TFile -> Promise<unit>
    abstract onLoadFile: file: TFile -> Promise<unit>
    abstract save: ?clear: bool -> Promise<unit>
    /// Gets the data from the editor. This will be called to save the editor contents to the file.
    abstract getViewData: unit -> string
    /// Set the data to the editor. This is used to load the file contents.
    /// 
    /// If clear is set, then it means we're opening a completely different file.
    /// In that case, you should call clear(), or implement a slightly more efficient
    /// clearing mechanism given the new data to be set.
    abstract setViewData: data: string * clear: bool -> unit
    /// Clear the editor. This is usually called when we're about to open a completely
    /// different file, so it's best to clear any editor states like undo-redo history,
    /// and any caches/indexes associated with the previous file contents.
    abstract clear: unit -> unit

/// This class implements a plaintext-based editable file view, which can be loaded and saved given an editor.
/// 
/// Note that by default, this view only saves when it's closing. To implement auto-save, your editor should
/// call `this.requestSave()` when the content is changed.
type [<AllowNullLiteral>] TextFileViewStatic =
    [<Emit "new $0($1...)">] abstract Create: leaf: WorkspaceLeaf -> TextFileView

type [<AllowNullLiteral>] TFile =
    inherit TAbstractFile
    abstract stat: FileStats with get, set
    abstract basename: string with get, set
    abstract extension: string with get, set

type [<AllowNullLiteral>] TFileStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> TFile

type [<AllowNullLiteral>] TFolder =
    inherit TAbstractFile
    abstract children: ResizeArray<TAbstractFile> with get, set
    abstract isRoot: unit -> bool

type [<AllowNullLiteral>] TFolderStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> TFolder

type [<AllowNullLiteral>] ToggleComponent =
    inherit ValueComponent<bool>
    abstract toggleEl: HTMLElement with get, set
    abstract setDisabled: disabled: bool -> ToggleComponent
    abstract getValue: unit -> bool
    abstract setValue: on: bool -> ToggleComponent
    abstract setTooltip: tooltip: string -> ToggleComponent
    abstract onClick: unit -> unit
    abstract onChange: callback: (bool -> obj option) -> ToggleComponent

type [<AllowNullLiteral>] ToggleComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: containerEl: HTMLElement -> ToggleComponent

type UserEvent =
    U4<MouseEvent, KeyboardEvent, TouchEvent, PointerEvent>

type [<AllowNullLiteral>] ValueComponent<'T> =
    inherit BaseComponent
    abstract registerOptionListener: listeners: Record<string, ('T -> 'T)> * key: string -> ValueComponent<'T>
    abstract getValue: unit -> 'T
    abstract setValue: value: 'T -> ValueComponent<'T>

type [<AllowNullLiteral>] ValueComponentStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> ValueComponent<'T>

type [<AllowNullLiteral>] Vault =
    inherit Events
    abstract adapter: DataAdapter with get, set
    /// Gets the path to the config folder.
    /// This value is typically `.obsidian` but it could be different.
    abstract configDir: string with get, set
    /// Gets the name of the vault
    abstract getName: unit -> string
    abstract getAbstractFileByPath: path: string -> TAbstractFile option
    abstract getRoot: unit -> TFolder
    abstract create: path: string * data: string * ?options: DataWriteOptions -> Promise<TFile>
    abstract createBinary: path: string * data: ArrayBuffer * ?options: DataWriteOptions -> Promise<TFile>
    abstract createFolder: path: string -> Promise<unit>
    abstract read: file: TFile -> Promise<string>
    abstract cachedRead: file: TFile -> Promise<string>
    abstract readBinary: file: TFile -> Promise<ArrayBuffer>
    abstract getResourcePath: file: TFile -> string
    /// <param name="file">- The file or folder to be deleted</param>
    /// <param name="force">- Should attempt to delete folder even if it has hidden children</param>
    abstract delete: file: TAbstractFile * ?force: bool -> Promise<unit>
    /// <summary>Tries to move to system trash. If that isn't successful/allowed, use local trash</summary>
    /// <param name="file">- The file or folder to be deleted</param>
    /// <param name="system">- Should move to system trash</param>
    abstract trash: file: TAbstractFile * system: bool -> Promise<unit>
    abstract rename: file: TAbstractFile * newPath: string -> Promise<unit>
    abstract modify: file: TFile * data: string * ?options: DataWriteOptions -> Promise<unit>
    abstract modifyBinary: file: TFile * data: ArrayBuffer * ?options: DataWriteOptions -> Promise<unit>
    abstract append: file: TFile * data: string * ?options: DataWriteOptions -> Promise<unit>
    abstract copy: file: TFile * newPath: string -> Promise<TFile>
    abstract getAllLoadedFiles: unit -> ResizeArray<TAbstractFile>
    abstract getMarkdownFiles: unit -> ResizeArray<TFile>
    abstract getFiles: unit -> ResizeArray<TFile>
    [<Emit "$0.on('create',$1,$2)">] abstract on_create: callback: (TAbstractFile -> obj option) * ?ctx: obj -> EventRef
    [<Emit "$0.on('modify',$1,$2)">] abstract on_modify: callback: (TAbstractFile -> obj option) * ?ctx: obj -> EventRef
    [<Emit "$0.on('delete',$1,$2)">] abstract on_delete: callback: (TAbstractFile -> obj option) * ?ctx: obj -> EventRef
    [<Emit "$0.on('rename',$1,$2)">] abstract on_rename: callback: (TAbstractFile -> string -> obj option) * ?ctx: obj -> EventRef
    [<Emit "$0.on('closed',$1,$2)">] abstract on_closed: callback: (unit -> obj option) * ?ctx: obj -> EventRef

type [<AllowNullLiteral>] VaultStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> Vault
    abstract recurseChildren: root: TFolder * cb: (TAbstractFile -> obj option) -> unit

type [<AllowNullLiteral>] View =
    inherit Component
    abstract app: App with get, set
    abstract icon: string with get, set
    abstract navigation: bool with get, set
    abstract leaf: WorkspaceLeaf with get, set
    abstract containerEl: HTMLElement with get, set
    abstract onOpen: unit -> Promise<unit>
    abstract onClose: unit -> Promise<unit>
    abstract getViewType: unit -> string
    abstract getState: unit -> obj option
    abstract setState: state: obj option * result: ViewStateResult -> Promise<unit>
    abstract getEphemeralState: unit -> obj option
    abstract setEphemeralState: state: obj option -> unit
    abstract getIcon: unit -> string
    /// Called when the size of this view is changed.
    abstract onResize: unit -> unit
    abstract getDisplayText: unit -> string
    abstract onHeaderMenu: menu: Menu -> unit

type [<AllowNullLiteral>] ViewStatic =
    [<Emit "new $0($1...)">] abstract Create: leaf: WorkspaceLeaf -> View

type [<AllowNullLiteral>] ViewCreator =
    [<Emit "$0($1...)">] abstract Invoke: leaf: WorkspaceLeaf -> View

type [<AllowNullLiteral>] ViewState =
    abstract ``type``: string with get, set
    abstract state: obj option with get, set
    abstract active: bool option with get, set
    abstract pinned: bool option with get, set
    abstract group: WorkspaceLeaf option with get, set

type [<AllowNullLiteral>] ViewStateResult =
    interface end

type [<AllowNullLiteral>] Workspace =
    inherit Events
    abstract leftSplit: U2<WorkspaceSidedock, WorkspaceMobileDrawer> with get, set
    abstract rightSplit: U2<WorkspaceSidedock, WorkspaceMobileDrawer> with get, set
    abstract leftRibbon: WorkspaceRibbon with get, set
    abstract rightRibbon: WorkspaceRibbon with get, set
    abstract rootSplit: WorkspaceSplit with get, set
    abstract activeLeaf: WorkspaceLeaf option with get, set
    abstract containerEl: HTMLElement with get, set
    abstract layoutReady: bool with get, set
    abstract requestSaveLayout: (unit -> unit) with get, set
    abstract requestSaveHistory: (unit -> unit) with get, set
    /// Runs the callback function right away if layout is already ready,
    /// or push it to a queue to be called later when layout is ready.
    abstract onLayoutReady: callback: (unit -> obj option) -> unit
    abstract changeLayout: workspace: obj option -> Promise<unit>
    abstract getLayout: unit -> obj option
    abstract createLeafInParent: parent: WorkspaceSplit * index: float -> WorkspaceLeaf
    abstract splitLeaf: source: WorkspaceItem * newLeaf: WorkspaceItem * ?direction: SplitDirection * ?before: bool -> unit
    abstract createLeafBySplit: leaf: WorkspaceLeaf * ?direction: SplitDirection * ?before: bool -> WorkspaceLeaf
    abstract splitActiveLeaf: ?direction: SplitDirection -> WorkspaceLeaf
    abstract splitLeafOrActive: ?leaf: WorkspaceLeaf * ?direction: SplitDirection -> WorkspaceLeaf
    abstract duplicateLeaf: leaf: WorkspaceLeaf * ?direction: SplitDirection -> Promise<WorkspaceLeaf>
    abstract getUnpinnedLeaf: ?``type``: string -> WorkspaceLeaf
    abstract getLeaf: ?newLeaf: bool -> WorkspaceLeaf
    abstract openLinkText: linktext: string * sourcePath: string * ?newLeaf: bool * ?openViewState: OpenViewState -> Promise<unit>
    /// <summary>Sets the active leaf</summary>
    /// <param name="leaf">- The new active leaf</param>
    /// <param name="pushHistory">- Whether to push the navigation history, or replace the current navigation history.</param>
    /// <param name="focus">- Whether to ask the leaf to focus.</param>
    abstract setActiveLeaf: leaf: WorkspaceLeaf * ?pushHistory: bool * ?focus: bool -> unit
    abstract getLeafById: id: string -> WorkspaceLeaf
    abstract getGroupLeaves: group: string -> ResizeArray<WorkspaceLeaf>
    abstract getMostRecentLeaf: unit -> WorkspaceLeaf
    abstract getLeftLeaf: split: bool -> WorkspaceLeaf
    abstract getRightLeaf: split: bool -> WorkspaceLeaf
    abstract getActiveViewOfType: ``type``: Constructor<'T> -> 'T option
    abstract getActiveFile: unit -> TFile option
    abstract iterateRootLeaves: callback: (WorkspaceLeaf -> obj option) -> unit
    abstract iterateAllLeaves: callback: (WorkspaceLeaf -> obj option) -> unit
    abstract getLeavesOfType: viewType: string -> ResizeArray<WorkspaceLeaf>
    abstract detachLeavesOfType: viewType: string -> unit
    abstract revealLeaf: leaf: WorkspaceLeaf -> unit
    abstract getLastOpenFiles: unit -> ResizeArray<string>
    /// Calling this function will update/reconfigure the options of all markdown panes.
    /// It is fairly expensive, so it should not be called frequently.
    abstract updateOptions: unit -> unit
    abstract iterateCodeMirrors: callback: (CodeMirror.Editor -> obj option) -> unit
    [<Emit "$0.on('quick-preview',$1,$2)">] abstract ``on_quick-preview``: callback: (TFile -> string -> obj option) * ?ctx: obj -> EventRef
    [<Emit "$0.on('resize',$1,$2)">] abstract on_resize: callback: (unit -> obj option) * ?ctx: obj -> EventRef
    [<Emit "$0.on('click',$1,$2)">] abstract on_click: callback: (MouseEvent -> obj option) * ?ctx: obj -> EventRef
    [<Emit "$0.on('active-leaf-change',$1,$2)">] abstract ``on_active-leaf-change``: callback: (WorkspaceLeaf option -> obj option) * ?ctx: obj -> EventRef
    [<Emit "$0.on('file-open',$1,$2)">] abstract ``on_file-open``: callback: (TFile option -> obj option) * ?ctx: obj -> EventRef
    [<Emit "$0.on('layout-change',$1,$2)">] abstract ``on_layout-change``: callback: (unit -> obj option) * ?ctx: obj -> EventRef
    /// Triggered when the CSS of the app has changed.
    [<Emit "$0.on('css-change',$1,$2)">] abstract ``on_css-change``: callback: (unit -> obj option) * ?ctx: obj -> EventRef
    /// Triggered when the user opens the context menu on a file.
    [<Emit "$0.on('file-menu',$1,$2)">] abstract ``on_file-menu``: callback: (Menu -> TAbstractFile -> string -> WorkspaceLeaf -> obj option) * ?ctx: obj -> EventRef
    /// Triggered when the user opens the context menu on an editor.
    [<Emit "$0.on('editor-menu',$1,$2)">] abstract ``on_editor-menu``: callback: (Menu -> Editor -> MarkdownView -> obj option) * ?ctx: obj -> EventRef
    /// Triggered when changes to an editor has been applied, either programmatically or from a user event.
    [<Emit "$0.on('editor-change',$1,$2)">] abstract ``on_editor-change``: callback: (Editor -> MarkdownView -> obj option) * ?ctx: obj -> EventRef
    /// Triggered when the editor receives a paste event.
    /// Check for `evt.defaultPrevented` before attempting to handle this event, and return if it has been already handled.
    /// Use `evt.preventDefault()` to indicate that you've handled the event.
    [<Emit "$0.on('editor-paste',$1,$2)">] abstract ``on_editor-paste``: callback: (ClipboardEvent -> Editor -> MarkdownView -> obj option) * ?ctx: obj -> EventRef
    /// Triggered when the editor receives a drop event.
    /// Check for `evt.defaultPrevented` before attempting to handle this event, and return if it has been already handled.
    /// Use `evt.preventDefault()` to indicate that you've handled the event.
    [<Emit "$0.on('editor-drop',$1,$2)">] abstract ``on_editor-drop``: callback: (DragEvent -> Editor -> MarkdownView -> obj option) * ?ctx: obj -> EventRef
    [<Emit "$0.on('codemirror',$1,$2)">] abstract on_codemirror: callback: (CodeMirror.Editor -> obj option) * ?ctx: obj -> EventRef
    /// Triggered when the app is about to quit. Not guaranteed to actually run.
    /// Perform some best effort cleanup here.
    [<Emit "$0.on('quit',$1,$2)">] abstract on_quit: callback: (Tasks -> obj option) * ?ctx: obj -> EventRef

type [<AllowNullLiteral>] WorkspaceStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> Workspace

type [<AllowNullLiteral>] WorkspaceItem =
    inherit Events
    abstract getRoot: unit -> WorkspaceItem

type [<AllowNullLiteral>] WorkspaceItemStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceItem

type [<AllowNullLiteral>] WorkspaceLeaf =
    inherit WorkspaceItem
    abstract view: View with get, set
    abstract openFile: file: TFile * ?openState: OpenViewState -> Promise<unit>
    abstract ``open``: view: View -> Promise<View>
    abstract getViewState: unit -> ViewState
    abstract setViewState: viewState: ViewState * ?eState: obj -> Promise<unit>
    abstract getEphemeralState: unit -> obj option
    abstract setEphemeralState: state: obj option -> unit
    abstract togglePinned: unit -> unit
    abstract setPinned: pinned: bool -> unit
    abstract setGroupMember: other: WorkspaceLeaf -> unit
    abstract setGroup: group: string -> unit
    abstract detach: unit -> unit
    abstract getIcon: unit -> string
    abstract getDisplayText: unit -> string
    abstract onResize: unit -> unit
    [<Emit "$0.on('pinned-change',$1,$2)">] abstract ``on_pinned-change``: callback: (bool -> obj option) * ?ctx: obj -> EventRef
    [<Emit "$0.on('group-change',$1,$2)">] abstract ``on_group-change``: callback: (string -> obj option) * ?ctx: obj -> EventRef

type [<AllowNullLiteral>] WorkspaceLeafStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceLeaf

type [<AllowNullLiteral>] WorkspaceMobileDrawer =
    inherit WorkspaceParent
    abstract collapsed: bool with get, set
    abstract expand: unit -> unit
    abstract collapse: unit -> unit
    abstract toggle: unit -> unit

type [<AllowNullLiteral>] WorkspaceMobileDrawerStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceMobileDrawer

type [<AllowNullLiteral>] WorkspaceParent =
    inherit WorkspaceItem

type [<AllowNullLiteral>] WorkspaceParentStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceParent

type [<AllowNullLiteral>] WorkspaceRibbon =
    interface end

type [<AllowNullLiteral>] WorkspaceRibbonStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceRibbon

type [<AllowNullLiteral>] WorkspaceSidedock =
    inherit WorkspaceSplit
    abstract collapsed: bool with get, set
    abstract toggle: unit -> unit
    abstract collapse: unit -> unit
    abstract expand: unit -> unit

type [<AllowNullLiteral>] WorkspaceSidedockStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceSidedock

type [<AllowNullLiteral>] WorkspaceSplit =
    inherit WorkspaceParent

type [<AllowNullLiteral>] WorkspaceSplitStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceSplit

type [<AllowNullLiteral>] WorkspaceTabs =
    inherit WorkspaceParent

type [<AllowNullLiteral>] WorkspaceTabsStatic =
    [<Emit "new $0($1...)">] abstract Create: unit -> WorkspaceTabs

type [<AllowNullLiteral>] Platform =
    /// The UI is in desktop mode.
    abstract isDesktop: bool with get, set
    /// The UI is in mobile mode.
    abstract isMobile: bool with get, set
    /// We're running the electron-based desktop app.
    abstract isDesktopApp: bool with get, set
    /// We're running the capacitor-js mobile app.
    abstract isMobileApp: bool with get, set
    /// We're running the iOS app.
    abstract isIosApp: bool with get, set
    /// We're running the Android app.
    abstract isAndroidApp: bool with get, set
    /// We're on a macOS device, or a device that pretends to be one (like iPhones and iPads).
    /// Typically used to detect whether to use command-based hotkeys vs ctrl-based hotkeys.
    abstract isMacOS: bool with get, set
    /// We're running in Safari.
    /// Typically used to provide workarounds for Safari bugs.
    abstract isSafari: bool with get, set

type [<AllowNullLiteral>] DomElementInfoAttr =
    [<Emit "$0[$1]{{=$2}}">] abstract Item: key: string -> U3<string, float, bool> option with get, set

type [<StringEnum>] [<RequireQualifiedAccess>] AjaxOptionsMethod =
    | [<CompiledName "GET">] GET
    | [<CompiledName "POST">] POST

type [<StringEnum>] [<RequireQualifiedAccess>] StatType =
    | File
    | Folder
