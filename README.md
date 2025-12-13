# 📧 .NET Email Verification API

A robust email verification system built with **ASP.NET Core 9.0**, **Entity Framework Core**, and **MySQL**. This API demonstrates user registration, email verification with token-based authentication, and secure login functionality.

---

## ✨ Features

- 📝 **User Registration** - Create new user accounts with automatic verification email
- ✉️ **Email Verification** - Token-based email verification with expiry (1 hour)
- 🔐 **Secure Login** - Login restricted to verified users only
- 🗄️ **MySQL Database** - Entity Framework Core with code-first migrations
- 📮 **SMTP Email Service** - Send verification emails using Gmail SMTP
- ⚡ **RESTful API** - Clean and simple endpoint design

---

## 🛠️ Technologies Used

- **Framework**: ASP.NET Core 9.0
- **Database**: MySQL 8.0
- **ORM**: Entity Framework Core 9.0
- **Email Service**: MailKit 4.14.1
- **Database Provider**: Pomelo.EntityFrameworkCore.MySql 9.0.0

---

## 📋 Prerequisites

Before running this project, make sure you have:

- ✅ [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- ✅ [MySQL Server](https://dev.mysql.com/downloads/) or WAMP/XAMPP
- ✅ Gmail account with [App Password](https://support.google.com/accounts/answer/185833)
- ✅ [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

---

## 🚀 Getting Started

### 1️⃣ Clone the Repository

```bash
git clone https://github.com/Dewkith-Ranathunga/dotnet-email-verification-api.git
cd dotnet-email-verification-api
```

### 2️⃣ Configure Database Connection

Create or update `appsettings.json` in `UserManagement.Api` folder:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=user_management;User=root;Password=;"
  }
}
```

### 3️⃣ Configure Email Settings

Update the `EmailSettings` in `appsettings.json`:

```json
{
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": "587",
    "Email": "your-gmail@gmail.com",
    "Password": "your-16-char-app-password"
  }
}
```

**📌 How to get Gmail App Password:**
1. Go to [Google Account Security](https://myaccount.google.com/security)
2. Enable **2-Step Verification**
3. Go to [App Passwords](https://myaccount.google.com/apppasswords)
4. Generate password for "Mail" → "Other (Custom name)"
5. Copy the 16-character password (remove spaces)

### 4️⃣ Install EF Core Tools

```bash
dotnet tool install --global dotnet-ef
```

### 5️⃣ Apply Database Migrations

```bash
cd UserManagement.Api
dotnet ef database update
```

### 6️⃣ Run the Application

```bash
dotnet run
```

🎉 API will be running at: **http://localhost:5077**

---

## 📡 API Endpoints

### 1. Register User

**`POST /api/users/register`**

Register a new user and send verification email.

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "password123"
}
```

**Response:**
```json
"Registered successfully. Please check your email to verify."
```

---

### 2. Verify Email

**`GET /api/users/verify?token={token}`**

Verify user email using the token sent to their email.

**Parameters:**
- `token` - Verification token from email

**Response:**
```json
"Email verified successfully!"
```

---

### 3. Login

**`POST /api/users/login?email={email}&password={password}`**

Login with verified email account.

**Parameters:**
- `email` - User email
- `password` - User password

**Response:**
```json
"Login Successful!"
```

**Error (if email not verified):**
```json
"Please verify your email first."
```

---

## 🗂️ Project Structure

```
User Management System/
├── UserManagement.Api/
│   ├── Controllers/
│   │   └── UsersController.cs      # API endpoints
│   ├── Data/
│   │   └── AppDbContext.cs         # EF Core DbContext
│   ├── Models/
│   │   └── User.cs                 # User entity model
│   ├── Migrations/                 # EF Core migrations
│   ├── Properties/
│   │   └── launchSettings.json     # Launch configuration
│   ├── appsettings.json            # App configuration (not in repo)
│   ├── Program.cs                  # Application entry point
│   └── UserManagement.Api.csproj   # Project file
├── .gitignore                      # Git ignore rules
└── README.md                       # This file
```

---

## 💾 Database Schema

**Users Table:**

| Column                   | Type         | Description                    |
|--------------------------|--------------|--------------------------------|
| `Id`                     | INT (PK)     | Auto-increment primary key     |
| `Name`                   | VARCHAR      | User's full name               |
| `Email`                  | VARCHAR      | User's email address           |
| `Password`               | VARCHAR      | User's password (plain text*)  |
| `IsEmailVerified`        | BOOL         | Email verification status      |
| `VerificationToken`      | VARCHAR      | Unique verification token      |
| `VerificationTokenExpiry`| DATETIME     | Token expiration time (1 hour) |

**\*Note:** In production, always hash passwords using bcrypt or similar.

---

## 🧪 Testing with Postman

### Test Flow:

1. **Register User** → User created with `IsEmailVerified = false`
2. **Check Database** → Copy `VerificationToken` from database
3. **Verify Email** → Use token in verify endpoint
4. **Check Database** → `IsEmailVerified = true`, token cleared
5. **Login** → Success! ✅

### Import Postman Collection:

Create requests for all 3 endpoints and test in order!

---

## 🔒 Security Notes

⚠️ **This is a learning project. For production use:**

- ✅ Hash passwords using **BCrypt** or **ASP.NET Core Identity**
- ✅ Use **HTTPS** in production
- ✅ Store `appsettings.json` credentials in **Azure Key Vault** or **environment variables**
- ✅ Add **rate limiting** to prevent spam
- ✅ Implement **JWT tokens** for authentication
- ✅ Add **input validation** and **sanitization**
- ✅ Use **refresh tokens** for better security

---

## 📝 License

This project is open-source and available under the [MIT License](LICENSE).

---

## 👨‍💻 Author

**Dewkith Ranathunga**

- GitHub: [@Dewkith-Ranathunga](https://github.com/Dewkith-Ranathunga)
- Repository: [dotnet-email-verification-api](https://github.com/Dewkith-Ranathunga/dotnet-email-verification-api)

---

## 🙏 Acknowledgments

- ASP.NET Core Documentation
- Entity Framework Core
- MailKit Library
- MySQL Database

---

## 📞 Support

If you have any questions or issues, please open an issue on GitHub.

---

**⭐ If you found this helpful, please give it a star!**
