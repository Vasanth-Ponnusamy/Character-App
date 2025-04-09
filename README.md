# 🚀 Character Management App (.NET 8)

This solution contains two main components:

- ✅ A **Console App** to fetch and sync data from the Rick and Morty public API.
- 🌐 A **Web App** to view, add, and filter characters with caching, pagination, and additional features.

---

## 🧱 Project Structure

- `CharacterApp.DataSync` - Console application to fetch alive characters and sync to SQL DB.
- `CharacterApp.Web` - MVC web application to display and manage characters.
- `CharacterApp.Data` - Handles DB access using Entity Framework Core.
- `CharacterApp.Tests` - Contains unit and integration test cases.

---

## 📦 Console App: `CharacterApp.DataSync`

### 🎯 Purpose

The console app fetches character data from the [Rick and Morty API](https://rickandmortyapi.com/api/character/) and stores only characters with the status `"Alive"` into a SQL database.

### ✅ Key Features

- Fetches all characters from the public API.
- Filters and stores only characters with status `"Alive"`.
- Uses `HttpClient` with `SocketsHttpHandler` for better performance and TLS security.
- Implements parallel HTTP requests for efficient pagination handling.
- Clears existing character, episode, and location data before inserting new data.
- Uses `Entity Framework Core` for ORM-based database interaction.

### 🔧 Sample Fetch Method

```csharp
public async Task<List<CharacterResponse>> FetchAllCharactersAsync()
{
    var tasks = new List<Task<string>>();
    // Fetch first page, then launch tasks for remaining pages
    ...
    var responses = await Task.WhenAll(tasks);
    ...
    return allCharacters.Where(x => x.Status == "Alive").ToList();
}
```
## 🌐 Web App: `CharacterApp.Web`

### 🎯 Purpose

The web application provides an interface to:

- Display a paginated list of "Alive" characters.
- Add new characters via form submission.
- Cache character list and avoid unnecessary API/database calls within a 5-minute window.
- Add a custom response header `from-database` to indicate if the characters were fetched from the database.

### 💡 Features

- Pagination support using `X.PagedList`.
- ViewBag-driven dropdowns for selecting origin and location.
- Data validation via model binding.
- Cache invalidation upon creating a new character.
- MVC Layered architecture: **Controller → Service → Repository**.

## 🗃️ Data Layer: `CharacterApp.Data`

### 🏗️ Architecture

The data access layer is built using **Entity Framework Core** and follows the **Repository Pattern** for clean separation of concerns and maintainability.

---

### 📋 Entities and Relationships

We have modeled three main entities:

- **Character**
- **LocationInfo**
- **Episode**

#### 🔁 Relationships

- A **Character**:
  - Has one **Origin** and one **Current Location** (both from the `LocationInfo` table).
  - Can appear in multiple **Episodes**. For simplicity, episode references are stored as a **comma-separated string of `Episode.Id` values** in the `Character` table.

---

### 🗂️ Repository Pattern

All data access logic is encapsulated within repository classes. The `CharacterRepository` handles operations related to characters, locations, and episodes.

#### ✅ Repository Responsibilities

- **InsertCharacterAsync(Character character)**  
  Inserts a new character into the database after resolving location and episode references.

- **InsertAndGetLocationInfo(LocationInfo info)**  
  Ensures location info is inserted only once and reused via lookup by name.

- **InsertAndGetEpisode(string episodeUrl)**  
  Parses the episode URL, checks if it exists, and inserts it if missing.

- **GetCharactersAsync()**  
  Returns the list of characters from the database including their mapped location/origin.

- **ClearData()**  
  Clears existing data from all relevant tables before performing a fresh data sync.
## 🧪 Testing: `CharacterApp.Tests`

The solution includes unit and integration test coverage across multiple layers using **xUnit**.

### ✅ Testing Strategy

- **Controller & Service Layers**
  - Used **xUnit** to validate behavior for controller actions and service methods.
  - Ensured correct pagination, caching behavior, and creation logic through mock services.

- **Repository Layer**
  - Used **Entity Framework Core In-Memory Database** to simulate real database behavior without hitting an actual SQL Server.
  - Tested all key operations including:
    - Character insertion and retrieval
    - Episode resolution
    - Location management

### 🧪 Tools & Frameworks

- `xUnit` for writing test cases
- `Moq` for mocking service/repo dependencies
- `Microsoft.EntityFrameworkCore.InMemory` for testing repository logic

---

The tests help ensure stability of the app during refactoring and catch edge cases in data processing and business logic.


