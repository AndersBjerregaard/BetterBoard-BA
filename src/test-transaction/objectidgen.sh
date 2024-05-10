generate_mongo_objectid() {
    characters="abcdefghijklmnopqrstuvwxyz0123456789"

    length=24

    for i in $(seq 1 $length); do
        echo -n "${characters:RANDOM % ${#characters}:1}"
    donecharacters=""
}

echo $(generate_mongo_objectid)