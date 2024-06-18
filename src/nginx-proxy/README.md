# Non-root user

Azure Container Apps does not support running containers as root users inside the container.

Therefore, some manual configuration must be done.

Original discussion https://forums.docker.com/t/running-nginx-official-image-as-non-root/135759/7 

# Configuration

Normal conf file is found within the container at path: `/etc/nginx/nginx.conf`

Default conf is found at path: `/etc/nginx/conf.d/default.conf`