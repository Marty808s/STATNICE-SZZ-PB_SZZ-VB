from db import getConnection
import pandas as pd
import matplotlib.pyplot as plt

conn = getConnection()

# cizinci v roce 2020 podle občanství
fetch1 = pd.read_sql(
    """
    SELECT 
        SUM(fc.pocet_cizinci) AS pocet_cizinci,
        c.nazev AS statni_obcanstvi,
        y.nazev AS rok
    FROM fact_cizinci AS fc
    JOIN dim_country AS c ON c.id = fc.fk_country
    JOIN dim_year    AS y ON y.id = fc.fk_year 
    WHERE y.nazev = '2020'
    GROUP BY c.nazev, y.nazev
    ORDER BY c.nazev;
    """,
    conn
)

# cizinci podle HDP země
fetch2 = pd.read_sql(
    """
    SELECT
      y.nazev              AS rok,
      c.nazev              AS statni_obcanstvi,
      fch.hdp_inhabitant   AS hdp_na_hlavu,
      SUM(fc.pocet_cizinci) AS pocet_cizinci
    FROM fact_cizinci      AS fc
    JOIN dim_year          AS y   ON y.id = fc.fk_year
    JOIN dim_country       AS c   ON c.id = fc.fk_country
    JOIN fact_country_hdp  AS fch ON fch.fk_country = fc.fk_country
                                  AND fch.fk_year    = fc.fk_year
    GROUP BY y.nazev, c.nazev, fch.hdp_inhabitant
    ORDER BY y.nazev, c.nazev;
    """,
    conn
)

print(fetch1.head())
print(fetch2.head())

#vizualizace:
# fetch 1
limit = 50
subset = fetch1.head(limit)

plt.figure(figsize=(12, 6))
plt.bar(subset["statni_obcanstvi"], subset["pocet_cizinci"])
plt.xticks(rotation=90)
plt.xlabel("Státní občanství")
plt.ylabel("Počet cizinců")
plt.title(f"Počet cizinců podle státního občanství (top {limit})")
plt.tight_layout()
plt.show()

