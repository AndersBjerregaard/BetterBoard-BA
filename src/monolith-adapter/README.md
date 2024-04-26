# Seed / Teardown of E2E tests on the BetterBoard monolith backend web api

For the sake of generating necessary test data, to perform E2E tests. Two endpoints was developed on the BB monolith backend web api, which - by principal - is the only application with write-rights to the database.
The code in these subdirectories are what new code was necessary, to achieve essential test data seed and teardown between each end-to-end tests.
Although the code seen here cannot compile alone, it functions as documentation for functional dependency in this E2E test system as a whole.