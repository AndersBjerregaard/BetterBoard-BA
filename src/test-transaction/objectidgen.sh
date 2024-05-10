generate_mongo_objectid() {
    length=24
    tr -dc 'a-f0-9' < /dev/urandom | head -c $length
}

echo $(generate_mongo_objectid)