-- Uygulama hesabı. Yalnızca ApartmanYonetimDB üzerinde okur ve yazar.
-- Parola bu dosyada durmaz; yerel App.config ile aynı değer çalıştırılırken verilir.
-- Host '%' sanal makine NAT'i içindir: masaüstü ve telefon aynı kapıdan gelir.

CREATE USER IF NOT EXISTS 'apartman_panel'@'%' IDENTIFIED BY @uygulama_parolasi;
ALTER USER 'apartman_panel'@'%' IDENTIFIED BY @uygulama_parolasi;

GRANT SELECT, INSERT, UPDATE, DELETE, CREATE, ALTER, INDEX
    ON ApartmanYonetimDB.*
    TO 'apartman_panel'@'%';

FLUSH PRIVILEGES;
