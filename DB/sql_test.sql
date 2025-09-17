SELECT s.jmeno, s.prijmeni, p.jmeno FROM student as s
JOIN studentpredmet AS sp ON s.id = sp.student
JOIN predmet AS p ON s.id = sp.predmet;

DROP VIEW IF EXISTS pocet_studentu;
CREATE VIEW pocet_studentu AS
SELECT sp.jmeno, SUM(p.pocetstudentu) FROM studijniprogram as sp
JOIN student AS s ON sp.id = s.studijniprogram
JOIN studentpredmet AS ssp ON sp.id = ssp.student
JOIN predmet AS p ON ssp.predmet = p.id
GROUP BY sp.jmeno
ORDER BY sp.jmeno ASC;


SELECT rocnik, STRING_AGG(jmeno, ', ') FROM predmet
GROUP BY rocnik

INSERT INTO predmet(jmeno, pocetstudentu, rocnik, semestr)
VALUES('TEst', 3, 2, 1),
('TEst', 3, 2, 1),
('TEst', 3, 2, 1);

CREATE TABLE pups(
	id SERIAL PRIMARY KEY,
	jmeno VARCHAR(32) NOT NULL,
	fk_student INT REFERENCES student(id)
)
