FROM nginxinc/nginx-unprivileged:1.27

RUN rm -fv /etc/nginx/conf.d/default.conf /etc/nginx/nginx.conf

COPY default.conf /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/nginx.conf