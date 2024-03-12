# ⚠️ Disclaimer ⚠️

The following document is written in Danish. As the target audience for my thesis is the institution of my university and its peers, as well as any potential Danish censor.

# Begrebsafklaring

Målgruppen for denne rapport regnes for at være artikulær inden for software miljøet. Derfor vil der, i denne rapport, være antagelser for at læseren har forhånds forståelse for centrale begreber og akronymer inden for software.
Andre begreber og akronymer kan der ses en beskrivelse for herunder:

| Begreb | Beskrivelse |
| ---- | ---- |
| BB | Akronym for *BetterBoard® ApS* |
| Monolit(ten) | BetterBoards primære softwareprodukt. En konjuktur mellem et *frontend*- og *backend* software projekt |
| IaC | Infrastructure as Code |

# Problemfelt

BB har (blandt andre) et primært softwareprodukt som de udstiller til deres kunder. Denne software versioneres på ét repository, og kan karakteriseres som en monolitisk sammenkobling af et *frontend* og *backend* software projekt.
Det er denne kodebase som ser de hyppigste ændringer.

BB har problemer med at opnå en ambition om at kunne *deploy* ændringer, lavet i monolitten, som minimum 16 gange om dagen.
Ændringer kan være:
- Nye Features
- Ændringer på eksisterende features
- Bugfixes

Udviklingsafdelingen får skabt nok ændringer i løbet af en dag, til ovenstående mål. Men den eksisterende CI / CD pipeline udfører kun *continuous delivery*, og ikke *continuous deployment*. Dette er et bevidst valg, da der ikke er høj nok tillid til den nuværende automatiske kvalitetssikring for software produktet.

Ydermere er der følgende flaskehals for at kunne opnå deployment målet: Pipelines eksekveringstiden. Den eksisterende eksekverings plan, består primært af at udnytte *self-hosted azure pipeline agents* på udviklernes host maskiner. Dette resulterer i en uskalerbar eksekverings plan, der ikke kan følge med ambitionerne.

Derudover er majoriteten af BB's software infrastruktur udokumenteret. Da det primært er blevet kreeret gennem brugergrænsefladen på Azure (som er BB's cloud provider). Dette har dannet problemer ift. at:
- Gennemsøge eksisterende konfigurationer.
- Ændre eksisterende infrastruktur.
- Vidensdele vedrørende infrastrukturen iblandt udviklingsafdelingen.

# Problemformulering

Denne opgave ønsker at opsætte kvantificerbare accept kriterier for en forbedret kvalitetssikringsproces, der kan garantere firmaets tillid til en automatisk *continuous deployment*. 
Derudover ekspandere på den eksisterende automatiske kvalitetssikring, for at opnå disse nye kriterier.
Opgaven ønsker ydermere at implementere en ny eksekverings plan for alle eksisterende- og potentielle Azure Pipelines.
Og sidst: Ønskes at, den mest kritiske software infrastruktur der står udokumenteret, skal i stedet dokumenteres i en passende form for *infrastructure as code* for firmaet. Samt, skal al ny infrastruktur berørt i denne opgave, følge samme *IaC*.
Opsummeret set undersøger denne opgave følgende:

    Hvordan kan BB sikre en høj kvalitet i det leverede produkt,
    når der er en ambition om at opdatere det 16 gange om dagen 
    - og ikke har et team af testere siddende.

Ud fra ovenstående udliciteres følgende underspørgsmål:

    Hvordan kan BB kvalitetssikre dets softwareprodukter, løbende og kontinuerligt når softwaren opdateres eller ændres, 
    før det når ud til målgruppen?

    Hvordan kan BB teste dets softwareprodukter på en vis der, 
    i så høj grad som muligt, kendetegner endebrugernes perspektiv?

    Hvad skal der til for at BB får dokumenteret deres software infrastruktur, 
    på en vis der er genskabelig og driftbar - i et udrulningsperspektiv?