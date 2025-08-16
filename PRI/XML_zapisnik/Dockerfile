#V PHP je DOM součástí extension dom (závisí na libxml). Do Dockerfile jsem přidal instalaci libxml2-dev a kompilaci xml a dom.

FROM php:8.2-apache

# instalace knihoven/extensions
RUN apt-get update \
    && apt-get install -y --no-install-recommends libxml2-dev \
    && docker-php-ext-install -j$(nproc) xml dom \
    && docker-php-ext-enable dom \
    && a2enmod rewrite \
    && rm -rf /var/lib/apt/lists/*

# vlastní apache vhost (DocumentRoot -> /var/www/html/php)
COPY apache/000-default.conf /etc/apache2/sites-available/000-default.conf