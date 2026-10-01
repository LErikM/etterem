-- Forrás adatbázis az "Étterem – Rendelés nyilvántartó API" önálló számonkéréshez
-- A vendeg tábla a diákoknak MÁR ADOTT kiindulási tábla.
-- A rendeles tábla létrehozása és feltöltése a diákok feladata.

CREATE DATABASE IF NOT EXISTS etterem
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE etterem;

CREATE TABLE IF NOT EXISTS vendeg (
  id INT AUTO_INCREMENT PRIMARY KEY,
  name VARCHAR(50) NOT NULL,
  email VARCHAR(100) NOT NULL,
  age INT NOT NULL,
  password VARCHAR(100) NOT NULL,
  registrationTime DATETIME NOT NULL
);

INSERT INTO vendeg (name, email, age, password, registrationTime) VALUES
('Németh Boglárka',  'nemeth.boglarka@example.com', 27, 'etterem1!',   '2025-01-08 18:30:00'),
('Farkas Ábel',        'farkas.abel@example.com',     31, 'vacsora25',   '2025-02-01 19:00:00'),
('Juhász Emese',       'juhasz.emese@example.com',    24, 'menu2025',    '2025-02-19 20:15:00'),
('Orsós Kende',        'orsos.kende@example.com',      29, 'asztal12',    '2025-03-10 17:45:00'),
('Rácz Hanna',         'racz.hanna@example.com',       26, 'pincer99',    '2025-03-27 21:05:00'),
('Tamás Botond',       'tamas.botond@example.com',     33, 'foglalas7',   '2025-04-15 18:50:00');

CREATE TABLE IF NOT EXISTS rendeles (
  id INT AUTO_INCREMENT PRIMARY KEY,
  dish VARCHAR(40) NOT NULL,
  description TEXT,
  orderTime DATETIME NOT NULL,
  updateTime DATETIME NOT NULL,
  vendegId INT NOT NULL,
  FOREIGN KEY (vendegId) REFERENCES vendeg(id)
);

INSERT INTO rendeles (dish, description, orderTime, updateTime, vendegId) VALUES
('Gulyásleves',         'Extra csípős kérésre, kenyérrel.',              '2025-05-02 12:30:00', '2025-05-02 12:30:00', 1),
('Rántott sajt',        'Hasábburgonyával és tartármártással.',          '2025-05-14 19:15:00', '2025-05-14 19:15:00', 2),
('Halászlé',            'Szegedi módra, csípős paprikával.',             '2025-05-26 13:00:00', '2025-05-26 13:00:00', 3),
('Túrós csusza',        'Tepertővel, dupla adag tejföllel.',             '2025-06-08 20:40:00', '2025-06-08 20:40:00', 5),
('Somlói galuska',      'Desszert, extra csokiöntettel.',                '2025-06-19 21:20:00', '2025-06-19 21:20:00', 6);
