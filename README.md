# DailyFoodSetApp 

**Author:** Xu Chengze  
**Module:** 6G6Z0014 - Mobile Computing  
**Framework:** .NET MAUI .NET 8.0 
**Tested Deployment Devices:** Windows PC, Android Smartphone, and Android Tablet (Cross-Platform Ready)

---

## 1. Project Overview
**DailyFoodSetApp** is a comprehensive, cross-platform mobile application developed using the .NET MAUI framework. Adhering to the required **Food and Drink** assessment theme, the application empowers users to discover foods, track precise nutritional information, dynamically plan daily meal targets, and log culinary moments. 

The project emphasizes a seamless, modern native user interface (UI), rigid client-side data validation, resilient remote REST API integrations with local data backup fallbacks, and extensive interactions with underlying mobile device hardware and accessibility frameworks to achieve enterprise-grade software standards.

---

## 2. Core Features

### 🍽️ UI/UX & Meal Discovery
* **Decoupled Architecture:** Built completely with clean XAML markup for precise UI rendering and decoupled code-behind logic.
* **Advanced Search & Filtering:** Offers asynchronous search functionality combined with multi-picker filters allowing users to refine meals by Category, Spiciness Level, Drink Sweetness, and Calorie Ranges.
* **Performance Optimization:** Features a lazy-loaded `CollectionView` combined with clean pagination controls (`RemainingItemsThresholdReached`) and pull-to-refresh (`RefreshView`) functionalities to ensure zero-lag scrolling and minimized network overloads.
* **Themed Visual Styling:** Employs a tailored, high-contrast palette consisting of warm culinary tones (Tomato Red, Basil Green, Creamy Custard) supporting complete runtime theme synchronization.

### 📝 Resilient Data Management
* **REST API Integration:** Connects seamlessly to a remote `mockapi.io` endpoint utilizing an asynchronous `HttpClient` engine for standard CRUD workflows (Fetching food collections, uploading new food details, and deleting records).
* **Local Fallback Solution:** To prevent disruptions during offline utilization or network drops, a secondary storage fallback is implemented. If the remote REST API becomes unreachable, the app automatically deserializes packaged local asset data (`Food.json`) gracefully without interrupting the user.

---

## 3. Advanced Mobile Hardware Utilization
The application interfaces with **6 distinct native hardware elements** via platform-specific abstract APIs, exceeding the highest assessment bracket requirements:

1.  **Camera:** Allows users to capture high-resolution images of meals directly within the platform using `MediaPicker.Default.CapturePhotoAsync()`.
2.  **Flash/Torch:** Features a toggleable high-intensity torch button (`Flashlight.Default.TurnOnAsync()`) to guarantee exposure and image clarity in dim dining environments.
3.  **Geolocation:** Pulls exact real-time sensor coordinate nodes (Latitude and Longitude values) under rigorous timeout constraints via `Geolocation.Default.GetLocationAsync()`.
4.  **Reverse Geocoding:** Converts retrieved physical coordinates into human-readable geographic locations (Country, Region, and City names) using `Geocoding.Default.GetPlacemarksAsync()`.
5.  **Accelerometer (Shake Gestures):** Registers spatial movement through the system accelerometer. Users can physically shake their smartphone device to trigger a randomized, curated meal prompt. Monitoring automatically untethers on page dismissal to enforce aggressive battery preservation.
6.  **Text-to-Speech (TTS):** Synthesizes structural linguistic properties to read complex nutritional profiles out loud. It integrates full cancellation token bindings so audio processes instantly terminate upon user demand or page displacement.
7.  **Vibration & Haptic Feedback:** Triggers distinct physical hardware pulses (`Vibration.Default.Vibrate()` and `HapticFeedback.Default.Perform()`) to give tactile confirmation during data entry validation errors and touch events.

---

## 4. Web Content Accessibility Guidelines (WCAG) Compliance
Accessibility (A11y) is a core component of this application's development lifecycle:
* **Dynamic Font Scaling:** Implements a custom structural traversal utility (`AccessibilityService`) that dynamically queries control nodes (`Label`, `Button`, `Entry`, `SearchBar`, `Picker`) and applies user-defined font size multiplier scales uniformly across the entire visual tree hierarchy at runtime.
* **Dark Mode Support:** Provides alternative application layout style dictionaries optimized for varying visual comfort settings and contrast standards.
* **Screen Reader Integration:** Integrates explicit `SemanticProperties.Hint`, `SemanticProperties.HeadingLevel` assignments, and programmatic spoken notifications via `SemanticScreenReader.Announce()` to inform visually impaired individuals of core state changes (e.g., list refreshes, theme alterations, and touch completions).

---

## 5. Robust Input Validation & Exception Handling
* **Strict UI Validation Form:** The item addition module strictly evaluates parameters prior to remote transmission. Food names must be non-empty, and calorie attributes are forcefully parsed against positive integer constraints.
* **Visual Alert Indicators:** Invalid data states immediately trigger dynamic UI error warnings accompanied by native hardware device vibration patterns to prompt rapid corrections.
* **Defensive Exception Management:** Heavy input/output boundaries, camera triggers, location lookups, and HTTP networking sequences are securely isolated within custom `try-catch-finally` block architectures, eliminating application crashes.

---

## 6. Directory & Code Structure
The implementation follows clean separation of concerns across Models, Services, Views, and localized resource assets:

```text
DailyFoodSetApp/
│
├── Models/
│   └── FoodItem.cs                 # Main data model representing structured meal definitions
│
├── Services/
│   ├── AccessibilityService.cs     # Text scaling algorithms and visual tree traversal engines
│   ├── FoodDataStore.cs            # Built-in local backup data seed matrix
│   ├── FoodService.cs              # HttpClient abstraction managing remote CRUD operations and offline fallbacks
│   └── MockApiConfig.cs            # Central configuration container for REST API URLs
│
├── Views/
│   ├── AddItemPage.xaml            # UI for meal creation forms
│   ├── AddItemPage.xaml.cs         # Form state assertion and input validation control rules
│   ├── FoodDetailPage.xaml         # UI displaying in-depth nutrient summary blocks
│   ├── FoodDetailPage.xaml.cs      # Detail presentation layer containing Text-to-Speech thread hooks
│   ├── FoodSearchPage.xaml         # UI with advanced search bars and filtered collection views
│   ├── FoodSearchPage.xaml.cs      # Core query handling logic, pagination control, and item templates
│   ├── HardwarePage.xaml           # UI detailing unified native hardware actions
│   ├── HardwarePage.xaml.cs        # Sensor implementations for Camera, Torch, and Location logging
│   ├── SettingsPage.xaml           # UI for accessibility preferences
│   └── SettingsPage.xaml.cs        # Logic for dark theme selection, scale factor changes, and vibration checks
│
├── App.xaml                        # Application initialization context and application styles
├── App.xaml.cs                     # Startup thread loading configurations and local system preferences
├── AppShell.xaml                   # Layout blueprint establishing TabBar shell structures
├── AppShell.xaml.cs                # Explicit global routing configuration map for views
│
└── DailyFoodSetApp.csproj          # Manifest defining target framework monickers and native permissions
