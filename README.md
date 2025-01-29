# ReserveApp
## 1.	Wstęp
Projekt jest aplikacją webową, umożliwiającą dokonywanie rezerwacji na zajęcia sportowe przez użytkowników  oraz ich zarządzanie przez administratora. System pozwala użytkownikom na przeglądanie dostępnych zajęć, dokonywanie rezerwacji, a także ich anulowanie. Administrator posiada dodatkowe uprawnienia, które umożliwiają mu zarządzanie zajęciami, wyświetlanie listy użytkowników zapisanych na poszczególne zajęcia oraz anulowanie rezerwacji użytkowników.
## 2.	Specyfikacja zewnętrzna
System posiada różne poziomy dostępu: zwykły użytkownik oraz administrator, który ma dodatkowe uprawnienia.
### Główne funkcjonalności
#### 1.	Rejestracja i logowanie użytkownika
*	Każdy użytkownik może się zarejestrować, podając nazwę użytkownika i hasło.
*	Po zalogowaniu użytkownik ma dostęp do swojego panelu, gdzie widzi swoje rezerwacje.
  
![Rezerwacje użytkownika](./images/scr1.png)
 
*	Przeglądanie szczegółów rezerwacji przez użytkownika

 ![Szczegóły rezerwacji](./images/scr2.png)

#### 2.	Przeglądanie dostępnych zajęć sportowych
*	Na stronie głównej użytkownik widzi listę zajęć sportowych wraz z informacjami o dacie, godzinie, czasie trwania oraz dostępnych miejscach.
*	Jeśli zajęcia nie są pełne, użytkownik może dokonać rezerwacji.

![Przeglądanie zajęć](./images/scr3.png)
 

#### 3.	Rezerwacja zajęć
*	Użytkownik może zarezerwować zajęcia, jeśli są jeszcze dostępne miejsca.
*	Po zapisaniu się na zajęcia użytkownik nie widzi już przycisku rezerwacji, a zamiast tego pojawia się informacja, że już jest zapisany.
*	Administrator ma możliwość przeglądania listy rezerwacji dla każdej klasy zajęć.

 ![Funkcja admina](./images/scr4.png)

#### 4.	Anulowanie rezerwacji
*	Użytkownik może anulować swoją rezerwację, a system automatycznie zwiększy liczbę dostępnych miejsc w bazie danych.
*	Administrator również ma możliwość anulowania rezerwacji innych użytkowników.

#### 5.	Panel administracyjny
*	Administrator może dodawać nowe zajęcia sportowe.
*	Może również przeglądać listy uczestników zajęć oraz anulować rezerwacje.
*	Dla administratora dostępny jest osobny link do Admin Panelu, widoczny w menu nawigacyjnym.

### Interfejs użytkownika
#### Główne menu nawigacyjne dostosowuje się do roli użytkownika:
*	Niezalogowany użytkownik widzi opcje: Login, Register, Classes.
*	Zalogowany użytkownik widzi opcje: Classes, My Reservations, Logout.
*	Administrator dodatkowo widzi: Admin Panel.

## 3.	Specyfikacja wewnętrzna
### Schemat bazy danych
  ![Funkcja admina](./images/schematBD.png)
  
### Infrastruktura wewnętrzna
*	Technologie backendowe: ASP.NET Core MVC, Entity Framework Core (9.0)
*	Baza danych: SQL Server 2022
*	Język programowania: C#
*	Architektura: Model-View-Controller (MVC)

### Wykorzystane biblioteki
W projekcie wykorzystano kilka kluczowych bibliotek, które wspierają jego funkcjonalność i bezpieczeństwo.
*	BCrypt.Net-Next 4.0.3 - służy do bezpiecznego przechowywania haseł użytkowników. Umożliwia stosowanie silnego mechanizmu hashującego BCrypt, który dodatkowo stosuje technikę "saltingu", co zwiększa odporność na ataki słownikowe i brute-force
*	Microsoft EntityFrameworkCore SqlServer 9.0.0 - jest to biblioteka ORM (Object-Relational Mapping), która pozwala na komunikację aplikacji z bazą danych SQL Server w sposób obiektowy. Dzięki niej operacje na bazie danych mogą być wykonywane za pomocą zapytań LINQ, co upraszcza zarządzanie danymi i eliminuje konieczność pisania skomplikowanych zapytań SQL
*	Microsoft EntityFrameworkCore Tools 9.0.0 - to zestaw narzędzi dla Entity Framework Core, który umożliwia zarządzanie migracjami bazy danych oraz generowanie schematów tabel. Ułatwia m.in. tworzenie i aktualizowanie struktury bazy danych bez konieczności ręcznego definiowania zapytań SQL.

