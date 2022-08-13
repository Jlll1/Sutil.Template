module Index

open Fable.Remoting.Client
open Shared
open Sutil
open Sutil.Styling

let adjectivesApi =
  Remoting.createApi ()
  |> Remoting.withRouteBuilder Route.builder
  |> Remoting.buildProxy<IAdjectivesApi>

type Msg =
  | AdjectiveChanged of string
  | AdjectiveChangeRequested

type Model =
  { Adjective : string }

let getAdjective m = m.Adjective

let init () =
  { Adjective = "" }, Cmd.ofMsg AdjectiveChangeRequested

let update msg model =
  match msg with
  | AdjectiveChanged a -> { model with Adjective = a }, Cmd.none
  | AdjectiveChangeRequested ->
      model, Cmd.OfAsync.perform adjectivesApi.getAdjective () AdjectiveChanged

open Sutil.Bulma
open Sutil.Attr

let view () =
  let model, dispatch = () |> Store.makeElmish init update ignore

  bulma.hero [
    hero.isFullheight
    Sutil.Attr.style [
      Css.backgroundColor "#c8f7c5"
      Css.cursorPointer
    ]
    onClick(fun _ -> dispatch AdjectiveChangeRequested) []

    bulma.heroBody [
      text.hasTextCentered
      bulma.container [
        bulma.columns [
          bulma.column [
            column.is6
            column.isOffset3
            bulma.title.h1 [
              Bind.fragment (model |> Store.map getAdjective)
                <| fun a -> Html.text $"Sutil is {a}!"
            ]
            bulma.subtitle.h3 [
              Html.text "For examples and documentation visit "
              Html.a [
                Attr.href "https://sutil.dev"
                Html.text "sutil.dev"
              ]
            ]
          ]
        ]
      ]
    ]
  ]

