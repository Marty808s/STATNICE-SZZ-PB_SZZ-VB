CREATE TABLE product (
    id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(255) UNIQUE NOT NULL,
    piece_unit_type ENUM('ks', 'm', 'kg'),
    stock BOOL DEFAULT 1,
    in_stock INT DEFAULT 0,
    description LONGTEXT,
    unit_price DECIMAL(12,4) NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE resource (
    id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    piece_unit_type ENUM('ks', 'm', 'kg'),
    win_stock INT,
    stock_alert INT DEFAULT 5,
    description LONGTEXT,
    PRIMARY KEY (id)
);

CREATE TABLE product_resource (
    id INT NOT NULL AUTO_INCREMENT,
    resource_id INT NOT NULL,
    product_id INT NOT NULL,
    piece_unit_type ENUM('ks', 'm', 'kg'),
    required_quantity INT NOT NULL,
    FOREIGN KEY (product_id) REFERENCES product(id) ON DELETE CASCADE,
    FOREIGN KEY (resource_id) REFERENCES resource(id) ON DELETE CASCADE,
    PRIMARY KEY (id)
);



CREATE TABLE customer (
    id INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL,
    surname VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE `order` (
    id INT NOT NULL AUTO_INCREMENT,
    customer_id INT NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (customer_id) REFERENCES customer(id) ON DELETE CASCADE
);

CREATE TABLE order_product (
    id INT NOT NULL AUTO_INCREMENT,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    unit_price DECIMAL(12,4) NOT NULL,
    quantity INT NOT NULL CHECK (quantity > 0),
    line_total DECIMAL(12,2) AS (ROUND(unit_price * quantity, 2)) STORED,
    currency CHAR(3) NOT NULL DEFAULT 'CZK',
    PRIMARY KEY (id),
    FOREIGN KEY (order_id) REFERENCES `order`(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES product(id) ON DELETE CASCADE
);