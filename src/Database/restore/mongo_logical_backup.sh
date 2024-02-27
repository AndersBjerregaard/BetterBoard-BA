#!/bin/bash

for i in $(ls ./architecture) ; do
        echo "Restoring collections from database: $i"
        while IFS= read -r line ; do
                echo "Restoring collection: $line"
                echo "mongodump --db $i --collection $line --query '{}' --out=/data/dump/logic"
                mongodump --db $i --collection $line --query '{ "foo": "bar" }' --out=/data/dump/logic
        done < "./architecture/$i"
done