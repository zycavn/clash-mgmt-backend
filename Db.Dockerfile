# Derived from mysql image (our base image)
FROM mysql/mysql-server

# Add the content of the _MySQL_Init_Script/ directory to your image
# All scripts in docker-entrypoint-initdb.d/ are automatically
# executed during container startup
COPY ./_MySQL_Init_Script/ /docker-entrypoint-initdb.d/