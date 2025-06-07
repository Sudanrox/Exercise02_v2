# Exercise02_v2

## 🔐 XML Data Protection Console App

This C# console application demonstrates basic security practices by protecting sensitive data in an XML file.

### ✅ Features
- **Encrypts** customer credit card numbers using AES encryption (can be decrypted later).
- **Hashes and salts** passwords using PBKDF2 with SHA-256 (secure and irreversible).
- Reads and modifies a sample XML file with customer data.
- Outputs a protected XML file as `customers_protected.xml`.

---

### 📂 Sample Input (`customers.xml`)
```xml
<?xml version="1.0" encoding="utf-8" ?>
<customers>
  <customer>
    <name>Bob Smith</name>
    <creditcard>1234-5678-9012-3456</creditcard>
    <password>Pa$$w0rd</password>
  </customer>
</customers>
🔒 Output (customers_protected.xml)
After running the application:

creditcard will be AES-encrypted.

password will be hashed using PBKDF2 + salt.

🧪 How to Build and Run
bash
Copy
Edit
dotnet build
dotnet run
Make sure customers.xml is placed in the same directory as your project file before running.

📁 Files Included
Program.cs – Main application logic

customers.xml – Sample input XML

customers_protected.xml – Output with protected data (auto-generated)

👨‍💻 Author
Sudanrox
GitHub: https://github.com/Sudanrox

📘 License
This project is for educational purposes only.

yaml
Copy
Edit

---

✅ After pasting this into a `README.md` file in your project folder, run:

```bash
git add README.md
git commit -m "Add README.md"
git push
