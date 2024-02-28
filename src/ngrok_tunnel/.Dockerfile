FROM ngrok/ngrok:3-alpine

USER root

RUN mkdir -vp /scripts

WORKDIR /scripts

COPY entrypoint.sh .

RUN chmod +x entrypoint.sh

USER ngrok

ENTRYPOINT [ "/scripts/entrypoint.sh" ]