# Dokumentacja aplikacji RentalCarApp 

## 1. Wymagania systemowe

### Minimalne wymagania programowe:
- **System operacyjny:** Windows 10 lub nowszy.
- **.NET SDK:** Wersja 6.0 lub nowsza.
- **Visual Studio 2022:** Z obsługą ASP.NET Core.
- **SQL Server:** Dowolna wersja wspierająca SQL Server Management Studio.
- **Przeglądarka internetowa:** Google Chrome, Microsoft Edge lub Firefox (najlepiej aktualna wersja).

---

## 2. Instalacja aplikacji

### Krok 1: Pobranie kodu
1. **Pobierz repozytorium projektu:**
   W terminalu lub wierszu poleceń wpisz:
   
   `git clone https://github.com/MBuchowicz1/RentalCarApp`
   
3. **Otwórz projekt:**
   Uruchom Visual Studio 2022 i załaduj pobrany projekt.

### Krok 2: Przygotowanie bazy danych
1. **Uruchom SQL Server Management Studio (SSMS):**
   Zaloguj się do instancji swojego serwera SQL.
2. **Utwórz nową bazę danych:**
   Wykonaj poniższe polecenie SQL:
  
   `CREATE DATABASE RentalCarDB;`

3. **Wykonaj migracje: **
   Otwórz konsolę Menedżera Pakietów w Visual Studio i wpisz:
   
   `Update-Database`
  
   Spowoduje to automatyczne utworzenie tabel w bazie danych.

### Krok 3: Konfiguracja łańcucha połączenia
1. **Edytuj plik `appsettings.json`:**
   W sekcji `ConnectionStrings` ustaw poprawny łańcuch połączenia do bazy danych:
   
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=RentalCarDB;Trusted_Connection=True;"
   }

   - `Server=localhost` może wymagać zmiany na nazwę twojej instancji SQL Server (np. `Server=.\SQLEXPRESS`).

### Krok 4: Uruchomienie aplikacji
1. **Ustaw projekt jako startowy:**
   Kliknij prawym przyciskiem myszy na projekt w Visual Studio i wybierz "Set as Startup Project".
2. **Uruchom aplikację:**
   Naciśnij klawisz **F5** lub wybierz "Run" z menu. Przeglądarka otworzy stronę pod adresem:
   
   `https://localhost:44361/`

---

## 3. Testowi użytkownicy

### Konto administratora:
- **Login:** admin@example.com
- **Hasło:** Admin@123
- **Rola:** Administrator. Posiada uprawnienia do zarządzania wszystkimi zasobami aplikacji.

### Konto użytkownika:
- **Login:** 123@123.123
- **Hasło:** 123!@#qweQWE
- **Rola:** Użytkownik. Posiada ograniczony dostęp do funkcji aplikacji, takich jak przeglądanie samochodów i tworzenie rezerwacji.

---

## 4. Opis funkcjonalności aplikacji

### **4.1 Panel logowania**
- Użytkownik loguje się za pomocą przypisanego loginu i hasła.
- Po zalogowaniu widzi funkcje dostępne zgodnie z przypisaną rolą (administrator lub użytkownik).

### **4.2 Funkcje dostępne dla administratora**
1. **Zarządzanie klientami:**
   - Możliwość dodawania nowych klientów.
   - Edycja istniejących klientów.
   - Usuwanie rekordów klientów.
2. **Zarządzanie samochodami:**
   - Dodawanie nowych pojazdów do bazy.
   - Edycja szczegółów pojazdów, takich jak marka, model czy cena dzienna.
   - Usuwanie pojazdów.
3. **Zarządzanie rezerwacjami:**
   - Tworzenie rezerwacji dla klientów.
   - Edytowanie i usuwanie istniejących rezerwacji.
4. **Zarządzanie płatnościami:**
   - Przeglądanie listy płatności.
   - Dodawanie nowych płatności.

### **4.3 Funkcje dostępne dla użytkownika standardowego**
1. **Przeglądanie samochodów:**
   - Użytkownik może przeglądać listę dostępnych pojazdów, wraz z ich szczegółami.
2. **Tworzenie rezerwacji:**
   - Użytkownik wybiera pojazd oraz daty wynajmu i składa rezerwację.

---

## 5. Formularze

### **5.1 Formularz rezerwacji**
- **Opis:** Formularz umożliwia utworzenie nowej rezerwacji.
- **Pola formularza:**
  - **Klient:** Wybór klienta z listy rozwijanej.
  - **Samochód:** Wybór pojazdu z listy rozwijanej.
  - **Data rozpoczęcia:** Pole daty.
  - **Data zakończenia:** Pole daty.
  - **Koszt całkowity:** Automatycznie obliczany na podstawie liczby dni i ceny dziennej pojazdu.

### **5.2 Formularz płatności**
- **Opis:** Formularz umożliwia dodanie płatności za rezerwację.
- **Pola formularza:**
  - **Rezerwacja:** Wybór rezerwacji z listy rozwijanej.
  - **Kwota:** Automatycznie wypełniana na podstawie kosztu rezerwacji.
  - **Status płatności:** Pole tekstowe (np. "Zakończona" lub "W trakcie").
  - **Data płatności:** Pole daty.

---

## 6. Szczegóły techniczne

### **6.1 Struktura bazy danych**
#### Tabela `Clients`
| Kolumna         | Typ danych | Uwagi                     |
|-----------------|------------|---------------------------|
| ID              | int        | Klucz główny             |
| FirstName       | nvarchar   | Imię klienta             |
| LastName        | nvarchar   | Nazwisko klienta         |
| Email           | nvarchar   | Adres e-mail             |
| Phone           | nvarchar   | Numer telefonu           |
| Address         | nvarchar   | Adres klienta            |

#### Tabela `Vehicles`
| Kolumna         | Typ danych | Uwagi                     |
|-----------------|------------|---------------------------|
| ID              | int        | Klucz główny             |
| Brand           | nvarchar   | Marka pojazdu            |
| Model           | nvarchar   | Model pojazdu            |
| Year            | int        | Rok produkcji            |
| DailyPrice      | decimal    | Cena za dzień wynajmu    |

#### Tabela `Reservations`
| Kolumna         | Typ danych | Uwagi                     |
|-----------------|------------|---------------------------|
| ID              | int        | Klucz główny             |
| ClientID        | int        | Klucz obcy do tabeli `Clients` |
| VehicleID       | int        | Klucz obcy do tabeli `Vehicles` |
| StartDate       | datetime   | Data rozpoczęcia         |
| EndDate         | datetime   | Data zakończenia         |
| TotalCost       | decimal    | Koszt całkowity wynajmu  |

#### Tabela `Payments`
| Kolumna         | Typ danych | Uwagi                     |
|-----------------|------------|---------------------------|
| ID              | int        | Klucz główny              |
| ReservationID   | int        | Klucz obcy do tabeli `Reservations` |
| Amount          | decimal    | Kwota płatności          |
| Status          | nvarchar   | Status płatności         |
| PaymentDate     | datetime   | Data realizacji płatności|

---

## 7. Testowanie
1. **Test logowania:**
   - Zaloguj się jako administrator i sprawdź dostęp do wszystkich funkcji zarządzania.
   - Zaloguj się jako użytkownik standardowy i upewnij się, że dostęp jest ograniczony tylko do przeglądania i tworzenia rezerwacji.
2. **Test rezerwacji:**
   - Utwórz rezerwację, sprawdzając poprawność obliczeń kosztu całkowitego.
   - Upewnij się, że formularz uniemożliwia utworzenie rezerwacji z błędnymi danymi (np. datą zakończenia wcześniejszą niż data rozpoczęcia).
3. **Test płatności:**
   - Dodaj płatność i sprawdź, czy kwota jest poprawnie przypisana na podstawie wybranej rezerwacji.
   - Sprawdź różne statusy płatności.

---

## 8. Wnioski i rekomendacje
Aplikacja jest gotowa do wdrożenia w środowisku produkcyjnym. Zalecenia:
- Regularnie twórz kopie zapasowe bazy danych, aby chronić dane użytkowników i rezerwacji.
- Monitoruj logi aplikacji, aby szybko reagować na ewentualne problemy.
- W przyszłości można rozważyć dodanie funkcji automatycznego powiadamiania e-mail o statusie rezerwacji i płatności.

