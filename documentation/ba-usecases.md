Forneden er en række *Use Cases*, identificeret individuelt ved forkortelsen *UC* og et unikt tal id.
De er grupperet efter hvad der menes at være enestående brugsscenarier. Da det giver det den bedste effektivitet, at de er kortfattet og specifikke.

Det er forstået således at alle *Use Cases* - medmindre andet eksplicit er skrevet - startes med at logge ind på portalen, med valide legitimationsoplysninger **uden** to-faktor godkendelse.

---

## UC 01 - Login

- Tilgå login siden
- Login på portalen med valide legitimationsoplysninger
- Verificer at en to-faktor godkendelses SMS er blevet sendt (\*) 
  - Her tænkes at verificere om [gatewayapi](https://gatewayapi.com/) responderer med success HTTP statuskode, på vores api-kald til deres service.
- Verificer at en to-faktor godkendelses SMS er blevet modtaget 
  - Her tænkes at opsætte et virtuelt telefonnummer. Således at en midlertidig container eller Azure Function applikation, fanger beskeden.
- Login med engangs koden som følge af to-faktor godkendelses SMS.

---

## UC 02 - Forside

- Login med en bruger der som minimum er medlem af én portal og ét datarum. 
  - Portalen for brugeren skal have: 
    - Dokumenter til underskrift
    - Kommende møde
    - Ulæste dokumenter
  - Datarummet skal have: 
    - Dokumenter til underskrift
- Verificer at portalen og datarummet er synlig på forsiden. 
  - Verificer at der er visuel indikation på at portalen har: 
    - Dokumenter til underskrift
    - Kommende møde
    - Ulæste dokumenter
  - Verificer at der er visuel indikation på at datarummet har: 
    - Dokumenter til underskrift
- Test søgefunktion på forsiden: Både overskrift og underoverskrift.
- Tilgå `dokumenter til underskrift` på forsiden. 
  - Verificer at der er dokumenter til underskrift. Samt at oprindelsen stemmer overens til portalen på forsiden.
- Tryk `tilbage`

---

## UC 03 - Møde Oprettelse

- Ud fra forsiden på en portal: Klik på `Opret møde`, i kolonnen til venstre.
- Verificer at man **ikke** kan oprette mødet for formularen er udfyldt med de nødvendige informationer. 
  - F.eks. test med ukorrekt tidspunkt: Et uangivet tidspunkt, eller starttidspunkt **efter** sluttidspunkt.
- Udfyld formularen tilstrækkeligt. 
  - Vælg én eller flere test brugere at sende notifikation til.
- Verificer at mailnotifikationen bliver afsendt. 
  - Her tænkes at verificere om [sparkpost](https://app.sparkpost.com/) responderer med success HTTP statuskode, på vores api-kald til deres service.
- Verificer at der kommer en notifikationsmail 
  - Inden for et givent tidsrum? Hvor længe er acceptabelt?

## UC 04 - Møde Agenda

- Udgangspunkt fra en bruger med en forside der har en portal med **mere end ét** kommende møde.
- Ved klik på den ovenstående portal, skal det først-kommende møde være startsiden.
- Under **AGENDA**, klik på `Opret fra skabelon`✏️ 
  - Vælg ud fra eksisterende skabelon: `Standard Agenda Skabelon (Bestyrelsesmøde)` (\*) 
    - Her skal der allerede være lavet en eksisterende skabelon på forhånd til testen.
- Klik på et givent mødepunkts `dropdown` pil ⬇️.
- Forsøg at uploade dokumenter (\*) 
  - ~~Upload fra~~ `Google Drev`~~?~~
  - Flere dokumenter på en gang? 
    - Virkelig mange? (**2 er fint**)
  - Hvilke dokumenttyper? 
    - Så vi får remt alle `convertere`. (**1 msg og 1 office fil**)
  - Hvor store / små? (**1 -5 mb - det skal bare være gængse størrelser**)
  - Hvad skal verificeres? (**at filen kommer op og kan ses efter et stykke tid**)
  - Hvis nogle tilfælde, hvad må ikke ske? 
  - Upload af filer direkte fra zip fil (uden udpakning)? (**test af man ikke kan uploade filer på 0 bytes**)
- Fremvisning af visse dokumenter i vieweren (\*) 
  - Hvilke dokumenter skal kunne vises? 
    - Gense office dokumenter (**test af de filer du uploader**)
    - ~~Billeder~~
  - Hvad er den forventet konverteringstid det tager fra et dokument er uploadet, til det kan ses i vieweren? 
    - Måske proportionelt til størrelsen? (**< 30s**)
- `Publicer` mødet (\*) 
  - SKal der noget verificering på dette? (**En anden bruger niveau 2 - skal kunne se agendaen på det publiserede møde**)

---

## UC 05 - Møde Referat

- Med udgangspunkt i et eksisterende møde uden et referat.
- Vælg en referent (samme testbruger som i øjeblikket tester?)
- `Dan referat` ved et mødepunkt, og indsæt noget tekst. 
  - Uden at trykke `gem`.
- Login med en anden bruger der også har adgang til selvsamme portal og møde. 
  - Verificer at brugeren **ikke** kan se referatet, eller danne et referat.
- Login med en admin på selvsamme portal. 
  - Verificer at adminen **ikke** kan danne referat, ved det punkt som i øjeblikket er ved at blive danne referat for.
- `Gem` her referatet som blev påbegyndt 3 steps over dette.
- Vælg en anden referent (en bruger med normal adgangsniveau / ikke admin)
- Login med brugeren som blev valgt som referent.
- Forsøg at danne referat- / ændre det referat punkt som blev dannet tidligere (\*) 
  - Her har der nogle gange været problemer med at referatet stadig er låst (som om at det stadig er ved at blive skrevet i) 
    - Måske låst på tids basis?
- Forsøg at danne `Dan referat` fra hele mødet. 
  - Det skulle brugeren gerne kunne
- Tilføj indkaldte personer
- Tilføj forhånds godkendere 
  - Verificer at netop kun referenten, admins og forhåndsgodkendere kan se referat kladden.
- Publiser referat 
  - Vælg personer til notifikation?

---

## UC 06 - Underskriftsproces (bør ikke testes i prod)

- Med udgangspunkt i en mappe i `Generelle Dokumenter` i en portal, med 4 forskellige underskriftbar dokumenter
- `Start ny underskriftproces` for dokumenterne. Med hver underskrift procestype: 
  - Simpel
  - PIN med SMS
  - MitID
  - BankID
- På en admin. Få verificeret at alle underskriftsprocesser dukker op på portalens `Dokumenter til underskrift`.
- Test underskrivning af hver type (\*) (**Vi venter lige med det her - da der er en økonomisk konsekvens ved at oprette signaturer i SCRIVe).**
  - Hertil er det stadig ubevidst hvorledes hver af disse kan verificeres 
    - Det tænkes at MitID og BankID udstiller development tests. Det må da være gjort før, da mange firmaer bruger dette. Og vil gerne teste om det virker, før de udruller det.
    - Eller om det er gennem **Scrive** at vi tester det. Om Scrive udstiller development API'er eller tests, e.l..
  - SMS testen kræver nok et virtuelt telefonnummer, som også blev benævnt under **UC 01**, til at fange SMS'en.

---

## UC 07 - Søgning Efter Filer

- Med udgangspunkt i en portal der har uploadet filer i møder, mapper, nøgletal / økonomi, projekter og generelle dokumenter. 
  - Alle disse dokumenter skal være unikke, både i titel og indhold.
- Verificer at ethver dokument kan fremsøges, både gennem titel og indhold.

---

## UC 08 - Afstemning (afventer afklaring)

---

## Andet

Dette punkt er til individuelle bemærkninger der blev gjort under demo mødet vedrørende disse use cases.
Når et dokument uploades et eller andet givent sted, fx i en mappe under et arbejdsrum. Er det ikke muligt at tilknytte dette dokument - øjeblikkeligt efter upload - i et møde, eller andet sted: Dokumentet er simpelthen ikke synligt endnu, ved tilknyt funktionen.

Lidt i samme boldgade som foroven: Det kræver formegentlig at *tvinge refresh af browser cache* når man uploader et dokument, og forventer at se det under "seneste dokumenter" i en portal, øjeblikkeligt efter upload.