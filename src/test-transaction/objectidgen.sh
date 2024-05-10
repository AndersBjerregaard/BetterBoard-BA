generate_mongo_objectid() {
    length=24
    objectid=$(tr -dc 'a-z0-9' < /dev/urandom | tr -d '\0')
    echo "${objectid:0:length}"
}

echo $(generate_mongo_objectid)