# ⚽ YallaKora — Sports Court Booking Platform

YallaKora is a web platform for booking sports courts, making it easy for users to book a court, join an existing match, or organize a match between two teams. The project is built with **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**.

---

## 📌 Overview

The project solves a common problem for casual football players: organizing informal matches (5-a-side, 7-a-side, 11-a-side) digitally, instead of relying on WhatsApp groups and phone calls to coordinate bookings and gather players.

---

## 🚀 Booking Types

The platform supports three different booking modes:

| Type | Description |
|---|---|
| **Individual** | Join an existing court and pick your position in the lineup |
| **With Friends** | Book the entire court for your group |
| **Team vs Team** | Challenge mode — book a slot and wait for another team to accept the challenge |

---

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core MVC (C#)
- **ORM:** Entity Framework Core
- **Database:** Microsoft SQL Server
- **Frontend:** Razor Views, HTML, CSS, Bootstrap
- **Session Management:** ASP.NET Core Session (handles login state and role-based access)

---

## 🗂️ Project Structure

```
YallaKora/
│
├── Controllers/        # Application logic (Booking, Profile, Account...)
├── Models/              # Entities (User, Booking, Slot, SlotPlayer, Review, Court)
├── Views/               # Razor views
├── Migrations/          # EF Core database migration history
├── wwwroot/             # Static files (images, CSS, JS)
└── MyContext.cs         # Main DbContext and relationship configuration
```

---

## 🗄️ Database Design

Key entities and their relationships:

- **User** → creates a **Booking**
- **Booking** → linked to a **Court** and owns a single **Slot**
- **Slot** → contains multiple **SlotPlayers** (joined players)
- **Review** → ratings/feedback exchanged between users after a match

The relationships between `Slot`, `User`, and `SlotPlayer` are configured so that deletion is handled manually in application code (application-level cascade) rather than relying entirely on SQL Server's cascade delete, in order to avoid Multiple Cascade Path conflicts caused by the interconnected relationships between tables.

---

## ✨ Key Features

- **Authentication system** using Sessions, with distinct permissions for Admin and User roles.
- **Position-based slot system** — each player selects their position in a slot, and no two players can take the same position.
- **Automatic booking cleanup** — if all players leave a slot, the related Slot and Booking are automatically removed.
- **User profile page** for viewing/editing account details or deleting the account.
- **Duplicate-join protection** — prevents a user from joining more than one position in the same slot.

---

## ⚙️ Getting Started

### Prerequisites
- Visual Studio 2022 (or newer)
- .NET SDK
- SQL Server (Local or Express)

### Steps

1. **Clone the repository**
```bash
git clone https://github.com/[username]/YallaKora.git
```

2. **Open the project in Visual Studio**
Open the `.sln` file located in the project root.

3. **Update the connection string**
In `MyContext.cs`, update the connection string to match your local SQL Server instance:
```csharp
string connectionString = "Server=[YOUR_SERVER];Database=YallaKora;Trusted_Connection=True;TrustServerCertificate=True;";
```

4. **Apply migrations**
Open the Package Manager Console and run:
```powershell
Update-Database
```

5. **Run the project**
Press F5 or Ctrl+F5 in Visual Studio.

---

## 🎯 Future Improvements

- Add password hashing and switch to ASP.NET Identity instead of manual session handling.
- Integrate an online payment system.
- Add real-time notifications for players when someone joins or leaves a match.
- Apply the Repository Pattern to separate data access logic from Controllers.

---

## 👤 Author

This project was developed as part of [course/ITI summer code camp].

**[Ahmed Gomaa]**
[(https://www.linkedin.com/in/ahmed-gomaa-a20407380/)]

---

## 📄 License

This is an educational project, free to use and modify.
