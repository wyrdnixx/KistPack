# KistPack

Anwendung zur Verwaltung und Dokumentation von Patienten-Chargen (Scannen, Suchen, PDF/CSV-Export).

---

## Versionshistorie

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
