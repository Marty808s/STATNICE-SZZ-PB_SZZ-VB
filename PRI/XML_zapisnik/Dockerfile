# Použijeme oficiální PHP 8 s Apachem
FROM php:8.3-apache

# Instalace systémových závislostí pro XML/XSL
RUN apt-get update && apt-get install -y --no-install-recommends \
        libxml2-dev \
        libxslt1-dev \
    && rm -rf /var/lib/apt/lists/*
