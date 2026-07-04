# KistPack

Anwendung zur Verwaltung und Dokumentation von Patienten-Chargen (Scannen, Suchen, PDF/CSV-Export).

ToDo:

- Kistpack Bug -> wurde bei manchen scanns nicht übernommen? konnte jetzt aber nicht nochmal reproduziert werden.

- Mehrteilige Akte
- Anzahl Elemente in Übersicht anzeigen (am besten pro Akten Typ (Nachlaufend...)
- Neulieferung nochmal prüfen - funktioniert das richtig.





---

## Versionshistorie


### Version 1.0.5.01
Bugfix: `NullReferenceException` in `searchWildcard()` behoben: `sqlQueryResult.Read()` wird jetzt auf Rückgabewert geprüft bevor auf Spalten zugegriffen wird

### Version 1.0.5.0
- Weiteren Typ angelegt: QS-Akte 
--> Eintrag in Settings in der DB

- Fallnummer prüfen nach Mandant -> nur richtige Fn / TT
--> Mandantenprüfung. Hart codiert (FN=0 / TT= 2) Aufwand über Settings-DB wäre höher


### Version 1.0.4.0
**Sicherheit**
- SQL-Injection-Schutz: Alle SQL-Queries in `ArchDB.cs` und `KistPackDB.cs` auf parametrisierte Befehle (`@Parameter`) umgestellt — betrifft `GetVisitFromArchive`, `searchPat`, `searchWildcard`, `saveDtToDB`, `testInsertDbTransaction`, `testInsertDb`
- Null-Koaleszenz für Umgebungsvariablen (`USERNAME`, `CLIENTNAME`, `COMPUTERNAME`) um Fehler bei fehlenden Werten zu verhindern

**Bugfixes**
- `NullReferenceException` in `insertNewVisit()` behoben: Das Setzen von `pv.Merkmal` (foreach über CheckedItems) wurde hinter den `null`-Check auf `pv` verschoben
- `String str = null`-Bug in der Duplikat-Prüfung behoben: Erste Iteration erzeugte `"nullCharge | ..."` — geändert zu `str = ""`
- `SqlDataReader`-Ressource-Leck in `ArchDB.GetVisitFromArchive` behoben: `dr.Close()` wurde im `HasRows == true`-Zweig nie erreicht — jetzt `using`-Block
- `SqlDataReader`-Lecks in `getKistPackDBVersion`, `getKistPackDBMerkmale`, `getKistPackDBExternalArchiveCall`, `searchPat`, `searchWildcard` behoben — alle Reader jetzt in `using`-Blöcken
- `databaseFileRead`: `sqlQueryResult.Read()` wird jetzt auf Rückgabewert geprüft bevor auf Spalten zugegriffen wird

**Performance**
- `searchWildcard`: Zwei separate Datenbankabfragen (N+1-Problem) zu einer einzigen Abfrage mit Subquery zusammengeführt
- `searchWildcard`: Äußeres `TOP(100)` entfernt — es konnte Chargen in der Mitte abschneiden und unvollständige Ergebnisse liefern; das innere `DISTINCT TOP(100)` begrenzt weiterhin auf maximal 100 verschiedene Chargen
- `searchPat`: `TOP(50)` als Sicherheitsnetz gegen unkontrolliert große Ergebnismengen hinzugefügt
- Suche (`tbSearchText_KeyPress`), PDF-Regenerierung (`btnRegenPDF_Click`) und CSV-Regenerierung (`btnRegenCSV_Click`) auf `async/await` umgestellt — Datenbankabfragen laufen jetzt auf dem Thread-Pool, UI bleibt während der Suche vollständig bedienbar

**UX**
- Während einer Suche: Suchfeld deaktiviert, Wartezeiger angezeigt; nach Abschluss Fokus automatisch zurück ins Suchfeld
- Zell-Highlighting in Suchergebnissen reaktiviert: Zellen, die den Suchtext enthalten, werden blau hinterlegt (`Color.LightSkyBlue`)

---

### Version 1.0.3.0
- Erneuter Upload der CSV-Datei wählbar

### Version 1.0.2.0
- Context-Menü zum Löschen von Einträgen, Ändern von Merkmalen und Aufruf des externen Archivs

### Version 1.0.1.0
- Suchergebnisse auf 100 Einträge begrenzt

---

## Datenbankeinrichtung

### Voraussetzung
Microsoft SQL Server (Express oder höher). Der Verbindungsstring wird in `App.config` bzw. den Programmeinstellungen hinterlegt:
```
Server = MSSQL; Database = KistPackDB; Trusted_Connection = True;
```

### Komplettes Setup-Script

Das folgende Script legt Datenbank, Tabellen, Indexes und initiale Konfigurationsdaten an.  
Es kann gefahrlos auf einer bestehenden Installation ausgeführt werden — alle Objekte werden nur erstellt wenn sie noch nicht vorhanden sind.

```sql
USE [master]
GO

/****** Datenbank anlegen ******/
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'KistPackDB')
BEGIN
    CREATE DATABASE [KistPackDB]
     CONTAINMENT = NONE
     ON PRIMARY
    ( NAME = N'KistPackDB',
      FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\KistPackDB.mdf',
      SIZE = 8192KB, MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
     LOG ON
    ( NAME = N'KistPackDB_log',
      FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\KistPackDB_log.ldf',
      SIZE = 8192KB, MAXSIZE = 2048GB, FILEGROWTH = 65536KB )
     WITH CATALOG_COLLATION = DATABASE_DEFAULT
END
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
    EXEC [KistPackDB].[dbo].[sp_fulltext_database] @action = 'enable'
GO

ALTER DATABASE [KistPackDB] SET RECOVERY SIMPLE
ALTER DATABASE [KistPackDB] SET AUTO_CLOSE OFF
ALTER DATABASE [KistPackDB] SET AUTO_SHRINK OFF
ALTER DATABASE [KistPackDB] SET AUTO_UPDATE_STATISTICS ON
ALTER DATABASE [KistPackDB] SET PAGE_VERIFY CHECKSUM
ALTER DATABASE [KistPackDB] SET TARGET_RECOVERY_TIME = 60 SECONDS
ALTER DATABASE [KistPackDB] SET READ_WRITE
GO

USE [KistPackDB]
GO

/****** Tabelle: Chargen ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Chargen]') AND type = N'U')
BEGIN
    CREATE TABLE [dbo].[Chargen] (
        [id]           INT          IDENTITY(1,1) NOT NULL,
        [Charge]       VARCHAR(50)  NOT NULL,
        [Kiste]        VARCHAR(50)  NOT NULL,
        [Merkmal]      VARCHAR(50)  NULL,
        [Fallnummer]   VARCHAR(50)  NOT NULL,
        [Person]       VARCHAR(50)  NULL,
        [Gebdat]       VARCHAR(50)  NULL,
        [Vorname]      VARCHAR(50)  NULL,
        [Nachname]     VARCHAR(50)  NULL,
        [Scandatum]    VARCHAR(50)  NULL,
        [Scanuser]     VARCHAR(50)  NULL,
        [Scanclient]   VARCHAR(50)  NULL,
        [Scanhostname] VARCHAR(50)  NULL,
        CONSTRAINT [PK_Chargen] PRIMARY KEY CLUSTERED ([id] ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF,
                  IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON,
                  ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END
GO

/****** Tabelle: Settings ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Settings]') AND type = N'U')
BEGIN
    CREATE TABLE [dbo].[Settings] (
        [Setting] VARCHAR(50)   NOT NULL,
        [Value]   VARCHAR(256)  NOT NULL
    ) ON [PRIMARY]
END
GO

/****** Indexes fuer Chargen-Tabelle ******/

-- Index auf Charge: beschleunigt den zweiten Teil der searchWildcard-Abfrage
-- (WHERE Charge IN (...)) und alle Abfragen die nach einer bestimmten Charge filtern
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Chargen]') AND name = N'IX_Chargen_Charge')
    CREATE NONCLUSTERED INDEX [IX_Chargen_Charge]
        ON [dbo].[Chargen] ([Charge] ASC)
GO

-- Index auf Fallnummer: beschleunigt searchPat (WHERE Fallnummer = @Fallnummer)
-- und die Duplikat-Pruefung beim Scannen
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Chargen]') AND name = N'IX_Chargen_Fallnummer')
    CREATE NONCLUSTERED INDEX [IX_Chargen_Fallnummer]
        ON [dbo].[Chargen] ([Fallnummer] ASC)
GO

/****** Initiale Konfigurationsdaten in Settings ******/
-- Werte nur einfuegen wenn der Eintrag noch nicht existiert
IF NOT EXISTS (SELECT 1 FROM [dbo].[Settings] WHERE [Setting] = 'DBVersion')
    INSERT INTO [dbo].[Settings] ([Setting], [Value]) VALUES ('DBVersion', '1.0.5.0')

IF NOT EXISTS (SELECT 1 FROM [dbo].[Settings] WHERE [Setting] = 'Merkmale')
    INSERT INTO [dbo].[Settings] ([Setting], [Value])
    VALUES ('Merkmale', 'Normal-Akte;Express-Akte;Nachlaufender-Befund;Neulieferung;QS-Akte')

-- Platzhalter: echten Aufruf entsprechend der Archiv-Software eintragen
-- Beispiel: echo cmd.exe /c ping #FALLNUMMER && pause
IF NOT EXISTS (SELECT 1 FROM [dbo].[Settings] WHERE [Setting] = 'ExternalArchiveCall')
    INSERT INTO [dbo].[Settings] ([Setting], [Value])
    VALUES ('ExternalArchiveCall', 'echo cmd.exe /c ping #FALLNUMMER && pause')
GO
```

### Hinweis zur Suchperformance

Die `LIKE '%suchtext%'`-Suche (führendes `%`) kann B-Tree-Indexes nicht nutzen und erzwingt immer einen vollständigen Tabellenscan auf den Textspalten (`Charge`, `Kiste`, `Vorname`, `Nachname` usw.).

Die angelegten Indexes helfen dennoch:
- **`IX_Chargen_Charge`** — der zweite Teil der Subquery (`WHERE Charge IN (...)`) profitiert direkt
- **`IX_Chargen_Fallnummer`** — exakte Suche bei `searchPat` wird vollständig per Index aufgelöst

Für sehr große Tabellen (> 500.000 Zeilen) wäre ein **SQL-Server-Volltextindex** (`CREATE FULLTEXT INDEX`) die nächste Stufe — erfordert aber Anpassungen an den Such-Queries.

---

## Offene Punkte (ToDo)

- [ ] Blaue Markierung nach Scan zurück auf erstes/Default-Item setzen
- [ ] Sicherung der aktuellen Charge in `%temp%` falls Programm abstürzt — beim Start prüfen und laden
- [ ] "Nachlaufender-Befund" und "Neulieferung" sind noch hart codiert (nicht konfigurierbar)

---

## Erledigte Punkte

- [x] Main: PDF-Pfad von Speicherdialog auf festen Pfad in `%temp%` umgestellt
- [x] Main: Beim Programmstart alte Dateien im Temp-Pfad aufräumen
- [x] Main: CSV-Datei erstellen (Pfad konfigurierbar über Programm-Config)
- [x] Main: PDF-Datei neu generierbar (statt in DB speichern)
- [x] Main: CSV-Datei neu generierbar (statt in DB speichern)
- [x] Suchfunktion: Grundsätzlich
- [x] Suchfunktion: PDF neu erstellen
- [x] Suchfunktion: CSV neu erstellen
- [x] Merkmale zu einer Akte setzen — Suchmaske, PDF, CSV
- [x] Merkmal für neuen Scan zurücksetzen
- [x] Prüfen ob bereits gescannte Akte für "Nachlaufenden Befund" gewählt werden soll
- [x] Merkmal-Änderung bei bereits gescannten Akten → Context-Menü
- [x] Merkmal "Nachlaufender Befund" automatisch setzen wenn Akte bereits versendet wurde
- [x] Externer Programmaufruf Archiv — Context-Menü — Start-Parameter in Settings-DB: `ExternalArchiveCall`
