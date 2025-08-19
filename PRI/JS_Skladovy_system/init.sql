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

-- přidat total cenu - agregace?
CREATE TABLE `order` (
    id INT NOT NULL AUTO_INCREMENT,
    customer_id INT NOT NULL,
    currency CHAR(3) NOT NULL DEFAULT 'CZK',
    total_price DECIMAL(20,2) NOT NULL DEFAULT 0,
    PRIMARY KEY (id),
    FOREIGN KEY (customer_id) REFERENCES customer(id) ON DELETE CASCADE
);

CREATE TABLE order_product (
    id INT NOT NULL AUTO_INCREMENT,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    unit_price DECIMAL(12,2) NOT NULL,
    quantity INT NOT NULL CHECK (quantity > 0),
    line_total DECIMAL(12,2) AS (ROUND(unit_price * quantity, 2)) STORED,
    PRIMARY KEY (id),
    FOREIGN KEY (order_id) REFERENCES `order`(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES product(id) ON DELETE CASCADE
);

CREATE TABLE users (
  id INT AUTO_INCREMENT PRIMARY KEY,
  username VARCHAR(255) UNIQUE NOT NULL,
  password_hash VARCHAR(255) NOT NULL,
  `role` ENUM('spravce','skladnik') NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- View s celkovou částkou objednávky spočítanou pomocí SELECT - VARIANTA A
CREATE VIEW order_with_total AS
SELECT
    o.id,
    o.customer_id,
    o.currency,
    COALESCE(SUM(op.line_total), 0) AS total_price
FROM `order` AS o
LEFT JOIN order_product AS op ON op.order_id = o.id
GROUP BY o.id, o.customer_id, o.currency;


-- Triggery udržující total_price v tabulce `order` - VARIANTA B
DELIMITER //

CREATE TRIGGER trg_order_product_ai AFTER INSERT ON order_product
FOR EACH ROW
BEGIN
    UPDATE `order` AS o
    SET o.total_price = (
        SELECT COALESCE(SUM(line_total), 0)
        FROM order_product
        WHERE order_id = NEW.order_id
    )
    WHERE o.id = NEW.order_id;
END//

CREATE TRIGGER trg_order_product_au AFTER UPDATE ON order_product
FOR EACH ROW
BEGIN
    IF OLD.order_id <> NEW.order_id THEN
        UPDATE `order` AS o
        SET o.total_price = (
            SELECT COALESCE(SUM(line_total), 0)
            FROM order_product
            WHERE order_id = OLD.order_id
        )
        WHERE o.id = OLD.order_id;
    END IF;

    UPDATE `order` AS o
    SET o.total_price = (
        SELECT COALESCE(SUM(line_total), 0)
        FROM order_product
        WHERE order_id = NEW.order_id
    )
    WHERE o.id = NEW.order_id;
END//

CREATE TRIGGER trg_order_product_ad AFTER DELETE ON order_product
FOR EACH ROW
BEGIN
    UPDATE `order` AS o
    SET o.total_price = (
        SELECT COALESCE(SUM(line_total), 0)
        FROM order_product
        WHERE order_id = OLD.order_id
    )
    WHERE o.id = OLD.order_id;
END//

DELIMITER ;

-- INSERTY
INSERT INTO users (username, password_hash, `role`)
VALUES ('spravce', 'spravce', 'spravce'),
       ('skladnik', 'skladnik', 'skladnik');

-- products
INSERT INTO product (name, piece_unit_type, stock, in_stock, description, unit_price)
VALUES
  ('Stůl Dub', 'ks', 1, 10, 'Masivní dubový stůl', 3499.9900),
  ('Židle Buk', 'ks', 1, 25, 'Buková židle s polstrováním', 899.5000);

-- resources
INSERT INTO resource (name, piece_unit_type, win_stock, stock_alert, description)
VALUES
  ('Šrouby M6', 'ks', 500, 50, 'Univerzální šrouby M6'),
  ('Deska dub 200x80', 'ks', 20, 5, 'Masivní dubová deska 200x80 cm');

INSERT INTO product_resource (resource_id, product_id, piece_unit_type, required_quantity)
VALUES
  (1, 1, 'ks', 20),  -- 20 šroubů na stůl
  (2, 1, 'ks', 1),   -- 1 deska na stůl
  (1, 2, 'ks', 8);   -- 8 šroubů na židli

-- customers
INSERT INTO customer (name, surname, email)
VALUES
  ('Jan', 'Novák', 'jan.novak@example.com'),
  ('Petr', 'Svoboda', 'petr.svoboda@example.com');

-- orders
-- Předpokládáme ID: Jan Novák = 1, Petr Svoboda = 2
INSERT INTO `order` (customer_id, currency)
VALUES
  (1, 'CZK'),
  (2, 'CZK');

INSERT INTO order_product (order_id, product_id, unit_price, quantity)
VALUES
  (1, 1, 3499.99, 1),  -- 1x Stůl Dub
  (1, 2, 899.50, 4),   -- 4x Židle Buk
  (2, 2, 899.50, 2),   -- 2x Židle Buk
  (2, 1, 3499.99, 1);  -- 1x Stůl Dub


-- SKLADNÍK
-- 1) Vytvoření role
CREATE ROLE IF NOT EXISTS 'skladnik';

-- 2) Čtecí práva (kromě tabulky users)
GRANT SELECT ON skladovy_system.* TO 'skladnik';
REVOKE SELECT ON skladovy_system.users FROM 'skladnik';

-- 3) Práce s objednávkami a skladem
GRANT INSERT, UPDATE ON skladovy_system.order_product TO 'skladnik';
GRANT UPDATE (in_stock) ON skladovy_system.product TO 'skladnik';
GRANT UPDATE (win_stock) ON skladovy_system.resource TO 'skladnik';

-- 4) Uživatelský účet a přiřazení role + default aktivace
CREATE USER IF NOT EXISTS 'skladnik'@'%' IDENTIFIED BY 'skladnik';
GRANT 'skladnik' TO 'skladnik'@'%';

-- SPRÁVCE
-- 1) Vytvoření role
CREATE ROLE IF NOT EXISTS 'spravce';
GRANT ALL ON skladovy_system.* TO 'spravce';

-- 2) Uživatelský účet a přiřazení role + default aktivace
CREATE USER IF NOT EXISTS 'spravce'@'%' IDENTIFIED BY 'spravce';
GRANT 'spravce' TO 'spravce'@'%';
