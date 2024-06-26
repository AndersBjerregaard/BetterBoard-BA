# Retrieve key from first input argument
key=$1

# Retrieve uuid from second input argument
id=$2

# Retrieve objectid from third input argument
objectid=$3

curl --location "https://devapi.betterboard.dk/api/v1/testtransaction/start" \
--header "X-Api-Key: $key" \
--header "Content-Type: application/json" \
--data "{
    "Id": "$id",
    "CompanyId": "$objectid"
}"