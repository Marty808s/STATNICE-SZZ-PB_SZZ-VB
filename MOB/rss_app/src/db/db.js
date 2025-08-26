import * as SQLite from 'expo-sqlite';
// https://docs.expo.dev/versions/latest/sdk/sqlite/

// zde bude práce s databází

// Funkce pro získání nového připojení
function getDb() {
    return SQLite.openDatabaseSync('myapp');
}


export async function createTables() {
    const db = getDb();

    const createFeedsTable = () => db.exec(`
        CREATE TABLE IF NOT EXISTS feeds(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            url TEXT,
            title TEXT,
        );
    `);

    const createContentTable = () => db.exec(`
        CREATE TABLE IF NOT EXISTS content(
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            feed_id INTEGER,
            title TEXT,
            description TEXT,
            link TEXT,
            published TEXT,
            comments TEXT
        );
    `);

    createFeedsTable();
    createContentTable();

}