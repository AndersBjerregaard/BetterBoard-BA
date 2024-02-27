FROM mongo:7.0

RUN mkdir -pv /data/dump/logical_backup

COPY ./logical_backup /data/dump/logical_backup

COPY ./entrypoint.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/entrypoint.sh

ENTRYPOINT [ "/usr/local/bin/entrypoint.sh" ]

CMD [ "mongod" ]