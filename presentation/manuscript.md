# Eksisterende

## WebDriver test op mod eksisterende løsning

- Window Driver Pattern
- Brug af relative XPath og eksplicitte waits så meget som muligt, for resiliente tests.
- Sørger for at dræbe sessionen hvis testen fejler (try-catch-finally)
- Headless
	- Ingen brugergrænseflade: Hurtige tests.

## Kørt i GitHub Actions

- Self-hosted runner
- Workflow fil
- Brug af GH Secrets
- Steps:
	- Generér UUID og Mongo ObjectID for workflow
	- State setup til acceptance tests
		- Brug af UUID og ObjectID fra step foroven
		- Brug af GH Secret til API Key
		- *Vis state setup metode senere*
	- Compile .XUnit projektet
	- Healthcheck Selenium Grid
		- *Vis Selenium grid senere*
	- Kør `dotnet test`
		- Kører Selenium Scripts parallelt
			- Hver test kører browserne parallelt
		- XUnit forsøger at bruge mængden af cpu tråde på den eksekverende maskine
	- Teardown
		- Ryd op efter State setup og tests.
		- Kører selv hvis nogle af de øvrige steps fejler.

## Selenium Grid

- Hub --> Nodes
	- Nodes er individuelle containere med en specifik browser og selenium webdriver
	- Hub fungerer som
		- reverse proxy for nodes
			- Sørger for kommunikation mellem selenium scripts og nodes
		- session queue for selenium scripts
			- Hvis der er flere queued tests end der er nodes
	- Parallel eksekvering op til mængden af nodes & browser instanser

## Infrastructure-as-Code

- .NET Aspire
	- Bicep IaC op mod Azure Container Apps
	- Konfigurering mellem lokalt udviklingsmiljø og IaC.
	- Fokus på udvikling
		- Mindre drift af adskillige miljøer
- Azure Container Apps
	- Managed Kubernetes Service
	- Mange af gevinsterne ved et container orkestrerings værktøj uden meget af kompleksiteten
		- Eksempelvis dynamisk skalering baseret på netværkstrafik
			- Skalering af nodes baseret på mængden af enqueued tests
			- Samt skalering til 0, uden for ulvetimerne

# Nyt

## Fejldump

- Tests kører ikke headless
	- Fordobler næsten test eksekveringstiden fra 1 minut til 2 minutter.
	- Holder sig stadig inden for rammerne om at kunne køre workflowet 8 gange i løbet af en arbejdsdag.
	- Gør lejlighed til at kunne optage hele tests
- Ved en fejlet test:
	- Tages et screenshot af skærmen
	- HTML'en for den nuværende DOM bliver gemt
	- Før sessionen dræbes
- Det skal være så let som muligt at fejlsøge hvad der forårsaget fejlen i acceptance testene.

## Proxy

- Et staging miljø 1-1 til produktionsmiljøet
- Azure Deployment Slots
- Kør tests på staging apps
- På baggrund af CI / CD best practices, skal artefakterne kun bygges **en** gang.
- En proxy efter selenium browser nodes
	- Til at ændre domænet på requests mellem frontend og backend

## Azure Pipeline

- Integrering til BB's CI / CD

## Infrastructure

- Azure KeyVault
- Nginx Proxy på ACA
- Managed Identity
- Selvkonfigureret Unprivileged Nginx image på private azure container registry
