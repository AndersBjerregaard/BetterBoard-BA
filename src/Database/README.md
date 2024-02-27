# Perform a logical snapshot

Create files with the names of the existing databases of the mongodb instance, into the `architecture` folder.
Populate the existing database file(s) with existing collection names separated by line breaks.
Make sure to use "LF" End of Line Sequence.

Copy the `mongo_logical_backup.sh` to a running instance of mongodb. As well as the `architecture` folder.
Example:
```bash
docker cp mongo_logical_backup.sh <container_id>:/usr/local/bin
docker cp architecture <container_id>:/usr/local/bin
```

Perform the restore:
```bash
docker exec -ti <container_id> /usr/local/bin/mongo_logical_backup.sh
```

A backup now exists in the direcotry `/data/dump/logic` in the instance where the shell script was executed.
You can copy the contents of this folder into your host system:
```bash
docker cp <container_id>:/data/dump/logic/ logical_backup
```

The `.Dockerfile` in this working directory will interpret the contents of `logical_backup` to run from a snapshot of that state.