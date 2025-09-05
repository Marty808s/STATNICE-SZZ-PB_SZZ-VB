select SUM(fc.pocet_cizinci), c.nazev, y.nazev from fact_cizinci as fc
join dim_country as c on c.id = fc.fk_country
join dim_year as y on y.id = fc.fk_year
where y.nazev = '2020'
group by c.nazev, y.nazev
order by c.nazev;


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