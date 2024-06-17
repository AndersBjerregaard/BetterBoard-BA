FROM traefik:v3.0
COPY traefik.yaml /etc/traefik/traefik.yaml
COPY dynamic.yaml /etc/traefik/dynamic/dynamic.yaml