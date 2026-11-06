# NativeAOT and trimming

## Library support

The library enables `IsAotCompatible` and trim analysis. Its built-in arrows, indicators, and tabs use expression-based bindings for the `CardsView` context and for custom contexts implementing `ICardsViewBindingContext`. The interface lists the properties those controls consume.

For backwards compatibility, custom contexts that do not implement the interface continue to use string-path bindings. Those legacy paths are marked `RequiresUnreferencedCode`; trimmed applications must preserve the bound properties. Implementing `ICardsViewBindingContext` enables statically analyzable bindings. Custom contexts should also implement `INotifyPropertyChanged` so updates to the bound values propagate. The library does not add broad linker descriptors or suppressions.

The built-in controls install a default binding only when the corresponding bindable property has not already been set by the application. They track which defaults they installed so a change between the supported binding-context kinds can refresh library-owned bindings without replacing configuration present before initialization.

The built-in indicator template uses a factory that creates `IndicatorItemView` directly. Sample item models and bindings are statically typed. `Styles.xaml` puts the non-Windows color in `OnPlatform.Default`, which preserves the fallback on iOS, Android, and Mac Catalyst under the MAUI 10 XAML compiler.

## Validation

Release builds of the library succeeded for `net10.0`, `net10.0-ios`, `net10.0-android`, and `net10.0-maccatalyst` with .NET SDK 10.0.403 and MAUI workload 10.0.110. The library reports five IL2026 warnings from the intentional legacy string-path compatibility branches. It reports no other IL2xxx or IL3xxx warnings. Existing compiler warnings include obsolete MAUI APIs (`ViewCell`, animation methods, and `VisualElement.Layout`).

A device publish with `PublishAot=true` succeeded for `ios-arm64`; the publish ran ILC and produced a native ARM64 executable without managed application DLLs. The resulting IPA was installed and launched on a physical iPhone. This confirms publish and startup; a complete interactive test matrix is not recorded.

An Android Release APK was published with `PublishAot=true`, installed on a physical device, and launched successfully. This confirms NativeAOT publishing and device startup for the sample; a complete interaction matrix was not recorded. Android NativeAOT is experimental in .NET MAUI.

A standard, non-NativeAOT iOS Simulator build launched and displayed the sample. This is a Mono simulator run and does not validate NativeAOT on the simulator. The installed .NET 10 iOS SDK rejects `iossimulator-arm64` for NativeAOT with `NETSDK1203` because simulator architecture RIDs are not supported for this publish scenario.

The iOS NativeAOT publish also reports linker/AOT diagnostics from Microsoft.Maui and its platform/compatibility dependencies, including IL2037, IL2104, and IL3053. These are dependency diagnostics, not warnings originating in the library. The library's five IL2026 warnings are the only IL2xxx/IL3xxx warnings attributed to `PanCardView` in the recorded builds. The sample demonstrates the default `CardsView` context. Runtime binding checks in the iOS Simulator used standard Debug/Mono compilation with `PublishAot=false`. They confirmed that `ArrowControl.ItemsCount = 3` survives attachment, an explicit `ItemsCount` binding retains its initial value and updates when the ViewModel changes, and explicit plus bound `ItemsSource` values in `IndicatorsControl` and `TabsControl` survive attachment and update. Animation callbacks were disabled in these checks to isolate binding behavior. Gesture navigation, indicator and tab taps, context replacement, and NativeAOT runtime behavior were not tested in that run.

## Device publish example

```sh
dotnet publish PanCardViewSample/PanCardViewSample/PanCardViewSample.csproj \
  -c Release -f net10.0-ios \
  -p:PublishAot=true
```

For device publishing, the iOS workload selects `ios-arm64`. Do not set `iossimulator-arm64` for NativeAOT publishing with the installed .NET 10 SDK.
