### Retrieves all estimates of issues that are "Done"
## Example output:
# 7.5
# "2024-04-02"
# 2.0
# 2024-04-02"
$DONE_ISSUES = gh api graphql -f query='
    query{
        node(id: "PVT_kwHOBVT5U84Aerum") {
            ... on ProjectV2 {
                items(first: 20) {
                    nodes{
                        id
                        fieldValues(first: 8) {
                            nodes{                
                                ... on ProjectV2ItemFieldTextValue {
                                    text
                                    field {
                                        ... on ProjectV2FieldCommon {
                                           name
                                        }
                                    }
                                }
                                ... on ProjectV2ItemFieldSingleSelectValue {
                                    name
                                    field {
                                        ... on ProjectV2FieldCommon {
                                            name
                                        }
                                    }
                                }
                                ... on ProjectV2ItemFieldNumberValue {
                                    number
                                    field {
                                        ... on ProjectV2FieldCommon {
                                            name
                                        }
                                    }
                                }
                                ... on ProjectV2ItemFieldDateValue {
                                    date
                                    field {
                                        ... on ProjectV2FieldCommon {
                                            name
                                        }
                                    }
                                }
                            }              
                        }
                    }
                }
            }
        }
    }' | jq -r '.data.node.items.nodes[] | select(.fieldValues.nodes[] | select(.field.name == "Status" and .name == "Done"))'

$ESTIMATE_DATES = $DONE_ISSUES | jq --slurp '.[] | .fieldValues.nodes[] | select(.field.name == "Date" or .field.name == "Estimate")'

$ESTIMATE_DATES | jq '.number + .date'