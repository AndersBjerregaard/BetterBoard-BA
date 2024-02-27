#!/bin/bash

# Start MongoDB
mongod --fork --logpath /var/log/mongodb.log --bind_ip_all

# Restore the backup
mongorestore /data/dump/logical_backup/

# Keep container running
tail -f /dev/null