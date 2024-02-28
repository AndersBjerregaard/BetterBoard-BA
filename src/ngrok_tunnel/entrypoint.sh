#!/bin/sh

echo "Starting tunnel for $TARGET_URI upstream against $NGROK_DOMAIN"
ngrok http $TARGET_URI --domain $NGROK_DOMAIN