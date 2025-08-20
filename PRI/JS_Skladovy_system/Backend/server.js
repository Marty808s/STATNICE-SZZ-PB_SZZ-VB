const express = require('express');
const mysql = require('mysql2') // jinak mi nešlo připojit (neprošel přístup pro ověření hesla..)
const cors = require('cors')

const app = express()
app.use(cors())
app.use(express.json())

const isLOCAL = true

const rootDB = mysql.createConnection({
    host: isLOCAL ? "localhost" : "db",
    user: "root",
    password: "root",
    database: "skladovy_system",
    charset: 'utf8mb4'
})

// tohle bude používat následně každá role - svůj string
let userDB = null;

// role je i password - v mém případě
const getUserConn = (role) => {
    const conn =  mysql.createConnection({
        host: isLOCAL ? "localhost" : "db",
        user: role,
        password: role, //tohle kdyžtak změnit
        database: "skladovy_system",
        charset: 'utf8mb4'
    });
    console.log("STRING CONN PRO ROLI", conn);
    return conn;
}

rootDB.connect((error) => {
    if (error) {
        console.error("[MySQL] Connection error:", error.code, error.message)
        console.error("[MySQL] Full error:", error)
        return
    }
    console.log("[MySQL] Connected, thread id:", rootDB.threadId)
})


rootDB.on('error', (err) => {
    console.error("[MySQL] Connection error event:", err)
})


// tady logika pro získání db con stringu pro příslušnou roli
//....

app.get('/', (req,res) => {
    return res.json("From BE");
})

// Healthcheck endpoint to verify DB connectivity
app.get('/health/db', (req, res) => {
    rootDB.ping((err) => {
        if (err) {
            console.error("[MySQL] Ping failed:", err)
            return res.status(500).json({ ok: false, error: err.message })
        }
        return res.json({ ok: true })
    })
})

app.listen(8081, () => {
    console.log("Listening");
})

app.get('/api/users', (req,res) => {
    const sql = 'SELECT id, username, role FROM users'
    rootDB.query(sql, (err, results) => {
        if (err) {
            console.error("[MySQL] /api/users query error:", err)
            return res.status(500).json({ error: err.message })
        }
        return res.json(results)
    })
}) 

app.post('/api/login', (req,res) => {
    const { username, password } = req.body || {}
    if (typeof username !== 'string' || typeof password !== 'string') {
        return res.status(400).json({ error: 'Invalid payload' })
    }

    //tady by mělo být hashování a porovnání s db hashem...

    const sql = 'SELECT id, username, role FROM users WHERE username = ? AND password_hash = ?'
    rootDB.execute(sql, [username, password], (err, results) => {
        if (err) {
            console.error("[MySQL] /api/login query error:", err)
            return res.status(500).json({ error: err.message })
        }
        if (!Array.isArray(results) || results.length === 0) {
            return res.status(401).json({ ok: false, error: 'Unauthorized' })
        }
        userDB = getUserConn(results[0].role)
        return res.json(results[0])
    })
}) 

app.get('/api/produkty', (req,res) => {
    const sql = 'SELECT * FROM product';
    //if (!userDB) {
    //    return res.status(401).json({ ok: false, error: 'Unauthorized' })
    //}
    rootDB.execute(sql, (err, results) => {
        if (err) {
            console.error("[MySQL] /api/produkty query error:", err)
            return res.status(500).json({ error: err.message })
        }
        return res.json(results)


    })
})

app.get('/api/objednavky', (req,res) => {
    const sql = 'SELECT o.id, c.name, c.surname, c.email, o.total_price, o.currency, o.state ' +
    'FROM `order` AS o ' +
    'JOIN customer AS c ON o.customer_id = c.id';
    //if (!userDB) {
    //    return res.status(401).json({ ok: false, error: 'Unauthorized' })
    //}
    rootDB.execute(sql, (err, results) => {
        if (err) {
            console.error("[MySQL] /api/objednavky query error:", err)
            return res.status(500).json({ error: err.message })
        }
        return res.json(results)


    })
})

app.get('/api/objednavky/:id', (req,res) => {
    const orderId = req.params.id;
    
    // Získání detailu objednávky s zákazníkem
    const orderSql = 'SELECT o.id, o.currency, o.total_price, o.state, c.name, c.surname, c.email ' +
                     'FROM `order` AS o ' +
                     'JOIN customer AS c ON o.customer_id = c.id ' +
                     'WHERE o.id = ?';
    
    // Získání produktů v objednávce
    const productsSql = 'SELECT op.id, op.unit_price, op.quantity, op.line_total, ' +
                        'p.id as product_id, p.name as product_name, p.piece_unit_type ' +
                        'FROM order_product AS op ' +
                        'JOIN product AS p ON op.product_id = p.id ' +
                        'WHERE op.order_id = ?';
    
    rootDB.execute(orderSql, [orderId], (err, orderResults) => {
        if (err) {
            console.error("[MySQL] /api/objednavky/:id order query error:", err)
            return res.status(500).json({ error: err.message })
        }
        
        if (!Array.isArray(orderResults) || orderResults.length === 0) {
            return res.status(404).json({ error: 'Objednávka nenalezena' })
        }
        
        const order = orderResults[0];
        
        rootDB.execute(productsSql, [orderId], (err, productResults) => {
            if (err) {
                console.error("[MySQL] /api/objednavky/:id products query error:", err)
                return res.status(500).json({ error: err.message })
            }
            
            const response = {
                order: order,
                products: productResults || []
            };
            
            return res.json(response);
        });
    });
})

// Customers list
app.get('/api/zakaznici', (req, res) => {
    const sql = 'SELECT id, name, surname, email FROM customer';
    rootDB.execute(sql, (err, results) => {
        if (err) {
            console.error("[MySQL] /api/zakaznici query error:", err)
            return res.status(500).json({ error: err.message })
        }
        return res.json(results)
    })
})

// Create order
app.post('/api/objednavky', (req, res) => {
    const { customer_id, currency = 'CZK' } = req.body || {};
    if (!customer_id) {
        return res.status(400).json({ error: 'Chybí customer_id' })
    }
    const sql = 'INSERT INTO `order` (customer_id, currency) VALUES (?, ?)';
    rootDB.execute(sql, [customer_id, currency], (err, result) => {
        if (err) {
            console.error("[MySQL] /api/objednavky (POST) query error:", err)
            return res.status(500).json({ error: err.message })
        }
        return res.status(201).json({ id: result.insertId })
    })
})

// Add products to order
app.post('/api/objednavky/:id/produkty', (req, res) => {
    const { id } = req.params;
    const items = Array.isArray(req.body?.items) ? req.body.items : [];
    if (!items.length) {
        return res.status(400).json({ error: 'Chybí položky objednávky' })
    }
    // Validate items
    for (const it of items) {
        if (!it.product_id || it.quantity === undefined || it.unit_price === undefined) {
            return res.status(400).json({ error: 'Každá položka musí obsahovat product_id, quantity, unit_price' })
        }
    }
    const sql = 'INSERT INTO order_product (order_id, product_id, unit_price, quantity) VALUES ?';
    const values = items.map(it => [Number(id), Number(it.product_id), Number(it.unit_price), Number(it.quantity)]);
    rootDB.query(sql, [values], (err) => {
        if (err) {
            console.error("[MySQL] /api/objednavky/:id/produkty (POST) query error:", err)
            return res.status(500).json({ error: err.message })
        }
        return res.json({ ok: true })
    })
})

// Update order
app.put('/api/objednavky/:id', (req, res) => {
    const { id } = req.params;
    const { customer_id, currency } = req.body || {};
    if (!customer_id) {
        return res.status(400).json({ error: 'Chybí customer_id' })
    }
    const sql = 'UPDATE `order` SET customer_id = ?, currency = ? WHERE id = ?';
    rootDB.execute(sql, [customer_id, currency, id], (err, result) => {
        if (err) {
            console.error("[MySQL] /api/objednavky/:id (PUT) query error:", err)
            return res.status(500).json({ error: err.message })
        }
        if (result.affectedRows === 0) {
            return res.status(404).json({ error: 'Objednávka nenalezena' })
        }
        return res.json({ ok: true })
    })
})

// Replace all products in order (delete existing + insert new)
app.put('/api/objednavky/:id/produkty', (req, res) => {
    const { id } = req.params;
    const items = Array.isArray(req.body?.items) ? req.body.items : [];
    
    // Validate items
    for (const it of items) {
        if (!it.product_id || it.quantity === undefined || it.unit_price === undefined) {
            return res.status(400).json({ error: 'Každá položka musí obsahovat product_id, quantity, unit_price' })
        }
    }
    
    // Start transaction
    rootDB.beginTransaction((err) => {
        if (err) {
            console.error("[MySQL] Transaction begin error:", err)
            return res.status(500).json({ error: err.message })
        }
        
        // Delete existing products
        rootDB.execute('DELETE FROM order_product WHERE order_id = ?', [id], (err) => {
            if (err) {
                return rootDB.rollback(() => {
                    console.error("[MySQL] Delete products error:", err)
                    res.status(500).json({ error: err.message })
                })
            }
            
            if (items.length === 0) {
                // No new items, just commit the deletion
                return rootDB.commit((err) => {
                    if (err) {
                        console.error("[MySQL] Commit error:", err)
                        return res.status(500).json({ error: err.message })
                    }
                    res.json({ ok: true })
                })
            }
            
            // Insert new products
            const sql = 'INSERT INTO order_product (order_id, product_id, unit_price, quantity) VALUES ?';
            const values = items.map(it => [Number(id), Number(it.product_id), Number(it.unit_price), Number(it.quantity)]);
            rootDB.query(sql, [values], (err) => {
                if (err) {
                    return rootDB.rollback(() => {
                        console.error("[MySQL] Insert products error:", err)
                        res.status(500).json({ error: err.message })
                    })
                }
                
                rootDB.commit((err) => {
                    if (err) {
                        console.error("[MySQL] Commit error:", err)
                        return res.status(500).json({ error: err.message })
                    }
                    res.json({ ok: true })
                })
            })
        })
    })
})

// Produkt - detail
app.get('/api/produkty/:id', (req, res) => {
	const { id } = req.params;
	const sql = 'SELECT * FROM product WHERE id = ?';
	rootDB.execute(sql, [id], (err, results) => {
		if (err) {
			console.error("[MySQL] /api/produkty/:id query error:", err)
			return res.status(500).json({ error: err.message })
		}
		if (!Array.isArray(results) || results.length === 0) {
			return res.status(404).json({ error: 'Produkt nenalezen' })
		}
		return res.json(results[0])
	})
})

// Produkt - vytvoření
app.post('/api/produkty', (req, res) => {
	const { name, piece_unit_type, in_stock = 0, description = null, unit_price } = req.body || {};
	if (!name || !piece_unit_type || unit_price === undefined) {
		return res.status(400).json({ error: 'Chybí povinná pole: name, piece_unit_type, unit_price' })
	}
	const validUnits = ['ks', 'm', 'kg'];
	if (!validUnits.includes(piece_unit_type)) {
		return res.status(400).json({ error: 'Neplatná jednotka piece_unit_type' })
	}
	const computedStock = Number(in_stock) > 0 ? 1 : 0;
	const sql = 'INSERT INTO product (name, piece_unit_type, in_stock, description, unit_price, stock) VALUES (?, ?, ?, ?, ?, ?)';
	rootDB.execute(sql, [name, piece_unit_type, Number(in_stock) || 0, description, Number(unit_price), computedStock], (err, result) => {
		if (err) {
			console.error("[MySQL] /api/produkty (POST) query error:", err)
			return res.status(500).json({ error: err.message })
		}
		return res.status(201).json({ id: result.insertId })
	})
})

// Produkt - úprava
app.put('/api/produkty/:id', (req, res) => {
	const { id } = req.params;
	const { name, piece_unit_type, in_stock, description = null, unit_price } = req.body || {};
	if (!name || !piece_unit_type || unit_price === undefined || in_stock === undefined) {
		return res.status(400).json({ error: 'Chybí povinná pole: name, piece_unit_type, in_stock, unit_price' })
	}
	const validUnits = ['ks', 'm', 'kg'];
	if (!validUnits.includes(piece_unit_type)) {
		return res.status(400).json({ error: 'Neplatná jednotka piece_unit_type' })
	}
	const sql = 'UPDATE product SET name = ?, piece_unit_type = ?, in_stock = ?, description = ?, unit_price = ? WHERE id = ?';
	rootDB.execute(sql, [name, piece_unit_type, Number(in_stock), description, Number(unit_price), id], (err, result) => {
		if (err) {
			console.error("[MySQL] /api/produkty/:id (PUT) query error:", err)
			return res.status(500).json({ error: err.message })
		}
		if (result.affectedRows === 0) {
			return res.status(404).json({ error: 'Produkt nenalezen' })
		}
		return res.json({ ok: true })
	})
})