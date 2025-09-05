-- dim country
CREATE TABLE IF NOT EXISTS dim_country (
    id SERIAL PRIMARY KEY,
    nazev TEXT NOT NULL UNIQUE
);

-- dim region
CREATE TABLE IF NOT EXISTS dim_region (
    id SERIAL PRIMARY KEY,
    nazev TEXT NOT NULL,
    uzemi TEXT NOT NULL,
    source_key INTEGER
);

-- dim year
CREATE TABLE IF NOT EXISTS dim_year(
    id SERIAL PRIMARY KEY,
    nazev TEXT NOT NULL
);

-- fact_cizinci
CREATE TABLE IF NOT EXISTS fact_cizinci(
    id SERIAL PRIMARY KEY,
    fk_country INTEGER REFERENCES dim_country(id),
    fk_region INTEGER REFERENCES dim_region(id),
    pocet_cizinci INTEGER,
    fk_year INTEGER REFERENCES dim_year(id),
    source_key INTEGER
);

-- fact_country_hdp
CREATE TABLE IF NOT EXISTS fact_country_hdp(
    id SERIAL PRIMARY KEY,
    fk_country INTEGER REFERENCES dim_country(id),
    hdp_inhabitant INTEGER,
    fk_year INTEGER REFERENCES dim_year(id)
);