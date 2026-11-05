# NativeAOT and trimming

The library targets .NET 10 and opts into `IsAotCompatible` and the trim analyzer. The SDK also enables the AOT and single-file analyzers through `IsAotCompatible`. MAUI normally disables trim analysis for projects that are not publishing with full trimming or NativeAOT; `_MauiDoNotConfigureTrimAnalyzer` keeps the library's explicit analyzer opt-in enabled for normal target builds.

## Library changes

- Internal bindings now use MAUI's expression-based binding APIs with explicit binding modes. This keeps binding members statically referenced and preserves the prior one-way, two-way, and one-way-to-source behavior.
- `CardsView.ItemsCount` keeps its public getter-only API. Its setter is assembly-internal so MAUI's binding source generator can emit its typed setter delegate; the library's binding is explicitly one-way.
- The default indicator `DataTemplate` uses a statically analyzable factory, and embedded-resource assembly access uses `typeof(ResourcesInfo).Assembly`.
- No broad linker descriptors or preservation attributes were added.

## Validation

On .NET SDK 10.0.403 with MAUI workload 10.0.110, Release builds of the library succeeded for `net10.0`, `net10.0-ios`, `net10.0-android`, and `net10.0-maccatalyst` with trimming and AOT analyzers enabled. They reported no IL2xxx or IL3xxx warnings from the library.

The sample's device NativeAOT publish completed for `ios-arm64` and produced an unsigned `PanCardViewSample.ipa`:

```sh
dotnet publish PanCardViewSample/PanCardViewSample/PanCardViewSample.csproj \
  -c Release -f net10.0-ios -r ios-arm64 \
  -p:PublishAot=true -p:EnableCodeSigning=false
```

Code signing was disabled, so that IPA was not installed or run on a device. This does not validate the app on a physical device.

The requested iOS Simulator NativeAOT publish was attempted with `iossimulator-arm64` and `PublishAot=true`. The installed .NET 10 iOS SDK rejected `dotnet publish` with:

```text
A runtime identifier for a device architecture must be specified in order to publish this project.
'iossimulator-arm64' is a simulator architecture.
```

A separate Release build for that simulator RID with `PublishAot=true` resolved `UseMonoRuntime=true` and did not select NativeAOT. It was stopped before treating its incomplete output as an app. A standard simulator Release build with signing enabled then succeeded, installed, and launched on the booted iPhone 17 Pro simulator (iOS 26.5). Its first screen rendered the sample demo list, the process remained running, and no app error/fault entries appeared in the checked simulator log interval. Direct touch automation was unavailable in this session, so carousel navigation, indicators, gestures, and animation were not interactively exercised. An earlier simulator launch with code signing disabled was rejected by iOS with `SIGKILL (Code Signature Invalid)`; the signed standard build launched successfully. These simulator results are a Mono/managed runtime regression check, not NativeAOT validation.

The repository contains no test project.

## Remaining warnings and limits

- The library builds still report six existing CS0618 warnings per target for `ViewCell`, `FadeTo`, `ScaleTo`, and `VisualElement.Layout`.
- The sample device publish reports IL2026 for string-path bindings in sample C# code, and IL2026/IL3050 for runtime XAML type resolution in generated XAML code. XAML binding compilation also reports XC0022 where sample bindings omit `x:DataType`.
- The publish reports IL2037 and IL3053 diagnostics from Microsoft.Maui compatibility/platform assemblies. These warnings originate outside this library.
- The sample's warnings mean this publish does not establish warning-free NativeAOT compatibility for the sample application. Applications using the library should compile their own bindings and avoid runtime XAML/type resolution patterns that MAUI marks as trim- or AOT-unsafe.
- The .NET 10 iOS SDK installed here does not support NativeAOT publish for `iossimulator-arm64`; only the device `ios-arm64` NativeAOT publish succeeded. Simulator NativeAOT execution therefore remains unvalidated.
- Windows was not built because this host is macOS and the project only includes its Windows target on Windows.
