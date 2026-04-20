

# CountCent

We all have desktop spreadsheets. Powerful apps for finances. But has anyone ever used it “on the way”, with a phone out of the pocket? Doesn’t sound right, does it? 

That’s why there is **CountCent** — a *simple, value-driven, personal finance tracking app for everyday use*, be it your PC, tablet or phone, at home in front of your desktop, or on the way to school — this is the companion to make your financial life easier, not harder.  


In summary, the CountCent app is a tool for users seeking a down-to-earth budget tracking app with least friction possible to manage their finances responsibly. 


## Table of Contents
1. [Features & Technologies Used](#features--technologies-used)
2. [Getting Started](#getting-started)
3. [Usage](#usage)
4. [Configuration & Deployment](#configuration--deployment)
5. [Contributing](#contributing)
6. [Project Status, Roadmap & Known Issues](#project-status-roadmap--known-issues)
7. [License & Acknowledgments](#license--acknowledgments)
8. [Contact/Support](#contactsupport)

## Features & Technologies Used
**Main Features:**
- Cross-platform: Use it on your laptop, tablet, or phone — whatever suits you best 
- Intuitive interface: You see that screen for the first time — and you inherently know what to do. Find it tricky? Refer to our demo! 
- Responsiveness on most screen sizes: A requirement, not a luxury in 2026
- Track daily expenses by entering amounts.
- Navigate through days using built-in date controls.
- View live equivalent rates for USD, GBP, JPY, and CHF against EUR.
- Delete individual expense entries (swipe or click).
- Calculate and display total daily expenses.
- Export all records to a local CSV file.
- Analyze all-time totals and daily averages.

**Tech Stack:**
- **Framework:** .NET MAUI (.NET 9)
- **Language:** C# 13.0, XAML
- **Database:** SQLite (LocalDbService)
- **Dependencies:** 
  - `CsvHelper` (for CSV export mapping)
  - `Frankfurter API` (for currency exchange rates)

## Getting Started

**Prerequisites:**
- Visual Studio 2022 (with .NET MAUI workload installed)
- .NET 9 SDK

**Installation Steps:**
1. Clone the repository:
```bash

   git clone https://github.com/valerkahere/CountCent.git

```

2. Open `CountCent.sln` in Visual Studio 2022.
3. Select your target emulator or local machine (Windows/Android).
4. Build and Run (F5).

## Usage
1. Open the app. The home screen defaults to "Today".
2. Type an expense amount (numbers only) into the entry field and press Enter.
3. The entry appears in the list below, and the Daily Total updates.
4. Swipe left on an item (mobile) or select it and click "Delete Entry" (desktop) to remove it.
5. Use the `<` and `>` buttons to navigate between different days.
6. Click "Export to a file" to save your data, then "Open Exported Data" to view the CSV.
7. Navigate to the "Analysis" tab to view all-time stats.

## Configuration & Deployment
- **API:** No API key is required. The app uses the open-source Frankfurter API (`https://api.frankfurter.dev/v1/`).
- **Database:** SQLite automatically creates a local `.db3` file on the device.

## Contributing
1. Fork the repository.
2. Create a feature branch (`git checkout -b feature/NewIdea`).
3. Commit your changes (`git commit -m 'Add NewIdea'`).
4. Push to the branch (`git push origin feature/NewIdea`).
5. Open a Pull Request.
6. Let me review it :O

## Project Status, Roadmap & Known Issues
**Status:** Active Development. 
**Roadmap:**
- Add custom category tagging for expenses.
- Implement weekly/monthly charts.
- Allow users to select their base currency (currently defaults to EUR).

**Known Issues:**
- Currency fallback defaults to 1:1 if the device is offline.

## License & Acknowledgments
**License:** MIT License
**Acknowledgments:**
- [Frankfurter API](https://frankfurter.dev/) for open-source currency rates.
- [CsvHelper](https://joshclose.github.io/CsvHelper/) for local data streaming.

## Contact/Support
For support or bug reports, please open an Issue on the [GitHub Repository](https://github.com/valerkahere/CountCent/issues).


