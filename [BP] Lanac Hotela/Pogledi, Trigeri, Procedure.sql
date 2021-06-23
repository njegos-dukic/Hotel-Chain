-- --------------------------------- --
-- Procedura: KreirajRacuneAranzmana --
-- --------------------------------- --

DROP PROCEDURE IF EXISTS KreirajRacuneAranzmana;
DELIMITER $$
CREATE PROCEDURE KreirajRacuneAranzmana() 
BEGIN
	DECLARE N INT DEFAULT 0;
	DECLARE I INT DEFAULT 0;
    DECLARE ARANZMANID INT DEFAULT 0;
    DECLARE CIJENAARANZMANA DOUBLE DEFAULT 0;
    
    SET SQL_SAFE_UPDATES = 0;
    
    DELETE FROM RACUN_ZA_ARANZMAN r
    WHERE r.racunAranzmanID IS NOT NULL;
    
	SET N = (SELECT COUNT(*) FROM ARANZMAN WHERE jeOtkazan = 0 AND jeZavrsen = 1);
	SET I = 0;
    
	WHILE I < N 
    DO 
		SET ARANZMANID = (SELECT a.aranzmanID FROM ARANZMAN a WHERE jeOtkazan = 0 AND jeZavrsen = 1 LIMIT I, 1);
        SET CIJENAARANZMANA = (SELECT (`Cijena nocenja` * `Ukupno nocenja`) FROM SviAranzmaniDetaljno WHERE `Aranzman ID` = ARANZMANID);
		INSERT INTO RACUN_ZA_ARANZMAN 
		VALUES (0, ARANZMANID, CIJENAARANZMANA);
		SET I = I + 1;
	END WHILE;
    
	DELETE RACUN_ZA_ARANZMAN
    FROM RACUN_ZA_ARANZMAN
    INNER JOIN ARANZMAN
    ON RACUN_ZA_ARANZMAN.aranzmanID = ARANZMAN.aranzmanID
    WHERE ARANZMAN.jeZavrsen = 0 OR ARANZMAN.jeOtkazan = 1;
END$$
DELIMITER ;

-- ------------------------- --
-- Procedura: ObrisiAranzman --
-- ------------------------- --

DROP PROCEDURE IF EXISTS ObrisiAranzman;
DELIMITER $$
CREATE PROCEDURE ObrisiAranzman(IN aranzmanZaUklanjanjeID INT) 
BEGIN
    SET SQL_SAFE_UPDATES = 0;
    DELETE RACUN_ZA_ARANZMAN FROM RACUN_ZA_ARANZMAN WHERE aranzmanID = aranzmanZaUklanjanjeID;
	DELETE ARANZMAN FROM ARANZMAN WHERE aranzmanID = aranzmanZaUklanjanjeID;
END$$
DELIMITER ;

-- ---------------------- --
-- Procedura: ObrisiGosta --
-- ---------------------- --

DROP PROCEDURE IF EXISTS ObrisiGosta;
DELIMITER $$
CREATE PROCEDURE ObrisiGosta(IN gostZaUklanjanjeID INT) 
BEGIN
    SET SQL_SAFE_UPDATES = 0;
    SET FOREIGN_KEY_CHECKS = 0;
    
    DELETE GOST FROM GOST WHERE gostID = gostZaUklanjanjeID;
    DELETE KONTAKT FROM KONTAKT WHERE gostID = gostZaUklanjanjeID;
	
    SET FOREIGN_KEY_CHECKS = 1;
END$$
DELIMITER ;

-- ---------------------- --
-- Procedura: ObrisiHotel --
-- ---------------------- --

DROP PROCEDURE IF EXISTS ObrisiHotel;
DELIMITER $$
CREATE PROCEDURE ObrisiHotel(IN removedHotelID INT) 
BEGIN
    SET SQL_SAFE_UPDATES = 0;
    SET FOREIGN_KEY_CHECKS = 0;
    
    UPDATE ARANZMAN SET jeOtkazan = 1, jeZavrsen = 0 WHERE hotelID = removedHotelID;
    CALL KreirajRacuneAranzmana();
    
    DELETE ARANZMAN FROM ARANZMAN WHERE hotelID = removedHotelID;
    DELETE SOBA FROM SOBA WHERE hotelID = removedHotelID;
    DELETE HOTEL FROM HOTEL WHERE hotelID = removedHotelID;
    DELETE KONTAKT FROM KONTAKT WHERE hotelID = removedHotelID;
    SET FOREIGN_KEY_CHECKS = 1;
END$$
DELIMITER ;

-- ---------------------- --
-- Procedura: ObrisiKontakt --
-- ---------------------- --

DROP PROCEDURE IF EXISTS ObrisiKontakt;
DELIMITER $$
CREATE PROCEDURE ObrisiKontakt(IN removeKontaktID INT) 
BEGIN
    SET SQL_SAFE_UPDATES = 0;
    SET FOREIGN_KEY_CHECKS = 0;
    
	DELETE KONTAKT FROM KONTAKT WHERE kontaktID = removeKontaktID;
    SET FOREIGN_KEY_CHECKS = 1;
END$$
DELIMITER ;


-- --------------------------- --
-- Triger: AranzmanAfterUpdate --
-- --------------------------- --

DROP TRIGGER IF EXISTS AranzmanAfterUpdate;
DELIMITER $$
CREATE TRIGGER AranzmanAfterUpdate 
AFTER UPDATE ON ARANZMAN 
FOR EACH ROW 
BEGIN
	IF NEW.jeZavrsen = 1 AND NEW.jeOtkazan = 0 THEN 
		INSERT INTO RACUN_ZA_ARANZMAN 
		VALUES (0, 
			   (NEW.aranzmanID), 
			   ((DATEDIFF(NEW.kraj, NEW.pocetak)) 
			   * 
			   (SELECT s.cijenaNocenja FROM SOBA s
					INNER JOIN ARANZMAN a
					ON a.sobaID = s.sobaID
					WHERE a.aranzmanID = NEW.aranzmanID)));
	END IF;
END$$
DELIMITER ;

-- ---------------------------- --
-- Triger: AranzmanBeforeInsert --
-- ---------------------------- --

DROP TRIGGER IF EXISTS AranzmanBeforeInsert;
DELIMITER $$
CREATE TRIGGER AranzmanBeforeInsert 
BEFORE INSERT ON ARANZMAN 
FOR EACH ROW
BEGIN
	DECLARE HOTELID INT;
	SET HOTELID = (SELECT s.hotelID FROM SOBA s WHERE s.sobaID = NEW.sobaID); 
    
    IF NEW.hotelID != HOTELID THEN
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'Soba mora biti iz odgovarajuceg hotela.';
    END IF;
    
	IF (DATEDIFF(NEW.kraj, NEW.pocetak)) <= 0 THEN 
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'Datum zavrsetka aranzmana mora biti nakon datuma pocetka.';
	END IF;
END$$
DELIMITER ;

-- ---------------------------- --
-- Triger: AranzmanBeforeUpdate --
-- ---------------------------- --

DROP TRIGGER IF EXISTS AranzmanBeforeUpdate;
DELIMITER $$
CREATE TRIGGER AranzmanBeforeUpdate 
BEFORE UPDATE ON ARANZMAN 
FOR EACH ROW
BEGIN
	DECLARE HOTELID INT;
	SET HOTELID = (SELECT s.hotelID FROM SOBA s WHERE s.sobaID = NEW.sobaID); 
    
    IF NEW.hotelID != HOTELID THEN
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'Soba mora biti iz odgovarajuceg hotela.';
    END IF;
    
	IF (DATEDIFF(NEW.kraj, NEW.pocetak)) <= 0 THEN 
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'Datum zavrsetka aranzmana mora biti nakon datuma pocetka.';
	END IF;
END$$
DELIMITER ;

-- --------------------------- --
-- Triger: KontaktBeforeDelete --
-- --------------------------- --

DROP TRIGGER IF EXISTS KontaktBeforeDelete;
DELIMITER $$
CREATE TRIGGER KontaktBeforeDelete 
BEFORE DELETE ON KONTAKT 
FOR EACH ROW 
BEGIN
	IF ((SELECT hotelID FROM HOTEL WHERE hotelID = OLD.hotelID) IS NOT NULL) THEN
		IF (SELECT COUNT(*) FROM KONTAKT WHERE hotelID = OLD.hotelID) = 1 OR (SELECT COUNT(*) FROM KONTAKT WHERE gostID = OLD.gostID) = 1 THEN 
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Nije moguce obrisati jedini kontakt.';
		END IF;
	END IF;
END$$
DELIMITER ;

-- --------------------------- --
-- Triger: KontaktBeforeInsert --
-- --------------------------- --

DROP TRIGGER IF EXISTS KontaktBeforeInsert;
DELIMITER $$
CREATE TRIGGER KontaktBeforeInsert 
BEFORE INSERT ON KONTAKT 
FOR EACH ROW 
BEGIN
	IF (NEW.hotelID IS NOT NULL AND NEW.gostID IS NOT NULL) THEN 
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'Kontakt moze pripadati samo gostu ili hotelu.';
	END IF;

    IF (NEW.tip != 'email' AND NEW.tip != 'telefon') THEN 
		SIGNAL SQLSTATE '45000'
		SET MESSAGE_TEXT = 'Kontakt mora biti email ili telefon.';
	END IF;
END$$
DELIMITER ;

-- --------------------------- --
-- Triger: KontaktBeforeUpdate --
-- --------------------------- --

DROP TRIGGER IF EXISTS KontaktBeforeUpdate;
DELIMITER $$
CREATE TRIGGER KontaktBeforeUpdate 
BEFORE UPDATE ON KONTAKT 
FOR EACH ROW 
BEGIN
	IF ((SELECT hotelID FROM HOTEL WHERE hotelID = OLD.hotelID) IS NOT NULL) THEN
		IF (SELECT COUNT(*) FROM KONTAKT WHERE hotelID = OLD.hotelID) = 1 OR (SELECT COUNT(*) FROM KONTAKT WHERE gostID = OLD.gostID) = 1 THEN 
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Nije moguce obrisati jedini kontakt.';
		END IF;
		
		IF (NEW.hotelID IS NOT NULL AND NEW.gostID IS NOT NULL) THEN 
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Kontakt moze pripadati samo gostu ili hotelu.';
		END IF;

		IF (NEW.tip != 'email' AND NEW.tip != 'telefon') THEN 
			SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Kontakt mora biti email ili telefon.';
		END IF;
	END IF;
END$$
DELIMITER ;

-- ---------------------------- --
-- Pogled: SviAranzmaniDetaljno --
-- ---------------------------- --

DROP VIEW IF EXISTS SviAranzmaniDetaljno;
CREATE VIEW SviAranzmaniDetaljno AS
SELECT h.hotelID AS 'Hotel ID',
	   a.aranzmanID AS 'Aranzman ID',
	   h.ime AS 'Hotel', 
       g.ime AS 'Ime', 
       g.prezime AS 'Prezime', 
	   a.pocetak AS 'Od', 
       a.kraj AS 'Do', 
       a.jeOtkazan AS 'Otkazan',
       a.jeZavrsen AS 'Zavrsen',
       a.sobaID AS 'Soba ID',
       s.cijenaNocenja AS 'Cijena nocenja', 
       DATEDIFF(a.kraj, a.pocetak) AS 'Ukupno nocenja'
FROM ARANZMAN a
INNER JOIN HOTEL h
	ON h.hotelID = a.hotelID
INNER JOIN GOST g
	ON g.gostID = a.gostID
INNER JOIN SOBA s
	ON a.sobaID = s.sobaID;

-- --------------------------------- --
-- Pogled: ZavrseniAranzmaniDetaljno --
-- --------------------------------- --

DROP VIEW IF EXISTS ZavrseniAranzmaniDetaljno;
CREATE VIEW ZavrseniAranzmaniDetaljno AS
SELECT sad.`Hotel ID` AS 'Hotel ID',
	   sad.`Aranzman ID` AS `Aranzman ID`,
       sad.`Hotel` AS 'Hotel', 
       sad.`Ime` AS 'Ime', 
       sad.`Prezime` AS 'Prezime', 
	   sad.`Od` AS 'Od', 
       sad.`Do` AS 'Do', 
       sad.`Soba ID` AS 'Soba ID',
       sad.`Cijena nocenja` AS 'Cijena nocenja', 
       sad.`Ukupno nocenja` AS 'Ukupno nocenja',
	   rza.cijena AS 'Cijena aranzmana'
FROM (SELECT * FROM SviAranzmaniDetaljno) sad
INNER JOIN RACUN_ZA_ARANZMAN rza
ON rza.aranzmanID = sad.`Aranzman ID`
WHERE sad.`Otkazan` = 0 AND sad.`Zavrsen` = 1;

-- ----------------------- --
-- Pogled: FinansijeHotela --
-- ----------------------- --

DROP VIEW IF EXISTS FinansijeHotela;
CREATE VIEW FinansijeHotela AS
SELECT ad.`Hotel ID` AS 'Hotel ID', 
	   ad.Hotel AS 'Hotel', 
       SUM(ad.`Ukupno nocenja`) AS 'Ukupno nocenja', 
	   SUM(ad.`Cijena aranzmana`) AS 'Ukupna zarada'
	  FROM (SELECT * FROM ZavrseniAranzmaniDetaljno) AS ad
	  GROUP BY ad.`Hotel ID`;
