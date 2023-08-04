#!/bin/bash

# Espera hasta que Camunda esté listo para recibir conexiones en el puerto 8080
until curl -sSf http://camunda:8080/camunda/app/welcome/default/; do
    echo "Camunda no está listo todavía. Esperando..."
    sleep 20
done

echo "Camunda está listo. Iniciando la aplicación .NET."
exec "$@"
