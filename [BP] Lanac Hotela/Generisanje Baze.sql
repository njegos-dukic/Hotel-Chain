-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema LanacHotela
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `LanacHotela` ;

-- -----------------------------------------------------
-- Schema LanacHotela
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `LanacHotela` DEFAULT CHARACTER SET utf8 ;
USE `LanacHotela` ;

-- -----------------------------------------------------
-- Table `LanacHotela`.`HOTEL`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`HOTEL` (
  `hotelID` INT NOT NULL AUTO_INCREMENT,
  `ime` VARCHAR(50) NOT NULL,
  `brojZvjezdica` INT NOT NULL,
  `ulica` VARCHAR(50) NOT NULL,
  `broj` VARCHAR(50) NOT NULL,
  `grad` VARCHAR(50) NOT NULL,
  `postanskiBroj` INT NOT NULL,
  `drzava` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`hotelID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`GOST`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`GOST` (
  `gostID` INT NOT NULL AUTO_INCREMENT,
  `JMBG` VARCHAR(13) NOT NULL,
  `ime` VARCHAR(50) NOT NULL,
  `prezime` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`gostID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`KONTAKT`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`KONTAKT` (
  `kontaktID` INT NOT NULL AUTO_INCREMENT,
  `tip` VARCHAR(10) NOT NULL,
  `info` VARCHAR(50) NOT NULL,
  `hotelID` INT NULL,
  `gostID` INT NULL,
  PRIMARY KEY (`kontaktID`),
  INDEX `fk_KONTAKT_HOTEL1_idx` (`hotelID` ASC) VISIBLE,
  INDEX `fk_KONTAKT_GOST1_idx` (`gostID` ASC) VISIBLE,
  CONSTRAINT `fk_KONTAKT_HOTEL1`
    FOREIGN KEY (`hotelID`)
    REFERENCES `LanacHotela`.`HOTEL` (`hotelID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_KONTAKT_GOST1`
    FOREIGN KEY (`gostID`)
    REFERENCES `LanacHotela`.`GOST` (`gostID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`SOBA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`SOBA` (
  `sobaID` INT NOT NULL AUTO_INCREMENT,
  `brojSprata` INT NOT NULL,
  `brojSobe` INT NOT NULL,
  `brojKreveta` INT NOT NULL,
  `imaTV` TINYINT NOT NULL,
  `imaKlimu` TINYINT NOT NULL,
  `cijenaNocenja` DECIMAL(5,2) NOT NULL,
  `hotelID` INT NOT NULL,
  PRIMARY KEY (`sobaID`),
  INDEX `fk_SOBA_HOTEL1_idx` (`hotelID` ASC) VISIBLE,
  CONSTRAINT `fk_SOBA_HOTEL1`
    FOREIGN KEY (`hotelID`)
    REFERENCES `LanacHotela`.`HOTEL` (`hotelID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`ARANZMAN`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`ARANZMAN` (
  `aranzmanID` INT NOT NULL AUTO_INCREMENT,
  `pocetak` DATETIME NOT NULL,
  `kraj` DATETIME NOT NULL,
  `jeOtkazan` TINYINT NOT NULL,
  `jeZavrsen` TINYINT NOT NULL,
  `hotelID` INT NOT NULL,
  `gostID` INT NOT NULL,
  `sobaID` INT NOT NULL,
  PRIMARY KEY (`aranzmanID`),
  INDEX `fk_ARANŽMAN_HOTEL1_idx` (`hotelID` ASC) VISIBLE,
  INDEX `fk_ARANŽMAN_GOST1_idx` (`gostID` ASC) VISIBLE,
  INDEX `fk_ARANŽMAN_SOBA1_idx` (`sobaID` ASC) VISIBLE,
  CONSTRAINT `fk_ARANŽMAN_HOTEL1`
    FOREIGN KEY (`hotelID`)
    REFERENCES `LanacHotela`.`HOTEL` (`hotelID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_ARANŽMAN_GOST1`
    FOREIGN KEY (`gostID`)
    REFERENCES `LanacHotela`.`GOST` (`gostID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_ARANŽMAN_SOBA1`
    FOREIGN KEY (`sobaID`)
    REFERENCES `LanacHotela`.`SOBA` (`sobaID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`USLUGA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`USLUGA` (
  `uslugaID` INT NOT NULL AUTO_INCREMENT,
  `cijena` DECIMAL(4,2) NOT NULL,
  `hotelID` INT NOT NULL,
  PRIMARY KEY (`uslugaID`),
  INDEX `fk_USLUGA_HOTEL1_idx` (`hotelID` ASC) VISIBLE,
  CONSTRAINT `fk_USLUGA_HOTEL1`
    FOREIGN KEY (`hotelID`)
    REFERENCES `LanacHotela`.`HOTEL` (`hotelID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`RESTORAN`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`RESTORAN` (
  `restoranID` INT NOT NULL AUTO_INCREMENT,
  `hotelID` INT NOT NULL,
  PRIMARY KEY (`restoranID`),
  INDEX `fk_RESTORAN_HOTEL1_idx` (`hotelID` ASC) VISIBLE,
  CONSTRAINT `fk_RESTORAN_HOTEL1`
    FOREIGN KEY (`hotelID`)
    REFERENCES `LanacHotela`.`HOTEL` (`hotelID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`MENI`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`MENI` (
  `meniID` INT NOT NULL AUTO_INCREMENT,
  `naziv` VARCHAR(50) NOT NULL,
  `od` DATETIME NULL,
  `do` DATETIME NULL,
  `restoranID` INT NOT NULL,
  PRIMARY KEY (`meniID`),
  INDEX `fk_MENI_RESTORAN1_idx` (`restoranID` ASC) VISIBLE,
  CONSTRAINT `fk_MENI_RESTORAN1`
    FOREIGN KEY (`restoranID`)
    REFERENCES `LanacHotela`.`RESTORAN` (`restoranID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`BAZEN`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`BAZEN` (
  `tipBazena` VARCHAR(20) NOT NULL,
  `uslugaID` INT NOT NULL,
  PRIMARY KEY (`uslugaID`),
  INDEX `fk_BAZEN_USLUGA1_idx` (`uslugaID` ASC) VISIBLE,
  CONSTRAINT `fk_BAZEN_USLUGA1`
    FOREIGN KEY (`uslugaID`)
    REFERENCES `LanacHotela`.`USLUGA` (`uslugaID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`TERETANA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`TERETANA` (
  `tipTeretane` VARCHAR(50) NOT NULL,
  `uslugaID` INT NOT NULL,
  PRIMARY KEY (`uslugaID`),
  INDEX `fk_TERETANA_USLUGA1_idx` (`uslugaID` ASC) VISIBLE,
  CONSTRAINT `fk_TERETANA_USLUGA1`
    FOREIGN KEY (`uslugaID`)
    REFERENCES `LanacHotela`.`USLUGA` (`uslugaID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`WELNESS_SPA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`WELNESS_SPA` (
  `tipResorta` VARCHAR(50) NOT NULL,
  `uslugaID` INT NOT NULL,
  PRIMARY KEY (`uslugaID`),
  INDEX `fk_WELNESS_SPA_USLUGA1_idx` (`uslugaID` ASC) VISIBLE,
  CONSTRAINT `fk_WELNESS_SPA_USLUGA1`
    FOREIGN KEY (`uslugaID`)
    REFERENCES `LanacHotela`.`USLUGA` (`uslugaID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`ZAPOSLENI`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`ZAPOSLENI` (
  `zaposleniID` INT NOT NULL AUTO_INCREMENT,
  `korisnickoIme` VARCHAR(50) NOT NULL,
  `lozinka` VARCHAR(512) NOT NULL,
  PRIMARY KEY (`zaposleniID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`RADNO_MJESTO`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`RADNO_MJESTO` (
  `radnoMjestoID` INT NOT NULL AUTO_INCREMENT,
  `opis` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`radnoMjestoID`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`RADNIK_U_HOTELU`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`RADNIK_U_HOTELU` (
  `hotelID` INT NOT NULL,
  `zaposleniID` INT NOT NULL,
  `plata` DECIMAL NOT NULL,
  `radnoMjestoID` INT NOT NULL,
  PRIMARY KEY (`hotelID`, `zaposleniID`),
  INDEX `fk_HOTEL_has_ZAPOSLENI_ZAPOSLENI1_idx` (`zaposleniID` ASC) VISIBLE,
  INDEX `fk_HOTEL_has_ZAPOSLENI_HOTEL1_idx` (`hotelID` ASC) VISIBLE,
  INDEX `fk_RADNIK_U_HOTELU_RADNO_MJESTO1_idx` (`radnoMjestoID` ASC) VISIBLE,
  CONSTRAINT `fk_HOTEL_has_ZAPOSLENI_HOTEL1`
    FOREIGN KEY (`hotelID`)
    REFERENCES `LanacHotela`.`HOTEL` (`hotelID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_HOTEL_has_ZAPOSLENI_ZAPOSLENI1`
    FOREIGN KEY (`zaposleniID`)
    REFERENCES `LanacHotela`.`ZAPOSLENI` (`zaposleniID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_RADNIK_U_HOTELU_RADNO_MJESTO1`
    FOREIGN KEY (`radnoMjestoID`)
    REFERENCES `LanacHotela`.`RADNO_MJESTO` (`radnoMjestoID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`STAVKA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`STAVKA` (
  `stavkaID` INT NOT NULL AUTO_INCREMENT,
  `meniID` INT NOT NULL,
  `cijena` DECIMAL(4,2) NOT NULL,
  `opis` VARCHAR(200) NULL,
  PRIMARY KEY (`stavkaID`),
  INDEX `fk_STAVKA_has_MENI_MENI1_idx` (`meniID` ASC) VISIBLE,
  CONSTRAINT `fk_STAVKA_has_MENI_MENI1`
    FOREIGN KEY (`meniID`)
    REFERENCES `LanacHotela`.`MENI` (`meniID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`RACUN`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`RACUN` (
  `racunID` INT NOT NULL AUTO_INCREMENT,
  `ukupnaCijena` DECIMAL(7,2) NOT NULL,
  `aranzmanID` INT NULL,
  PRIMARY KEY (`racunID`),
  INDEX `fk_RAČUN_ARANŽMAN1_idx` (`aranzmanID` ASC) VISIBLE,
  CONSTRAINT `fk_RAČUN_ARANŽMAN1`
    FOREIGN KEY (`aranzmanID`)
    REFERENCES `LanacHotela`.`ARANZMAN` (`aranzmanID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`USLUGA_NA_RACUNU`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`USLUGA_NA_RACUNU` (
  `uslugaID` INT NOT NULL,
  `računID` INT NOT NULL,
  `cijena` DECIMAL(4,2) NOT NULL,
  `kolicina` INT NOT NULL,
  PRIMARY KEY (`uslugaID`, `računID`),
  INDEX `fk_USLUGA_has_RAČUN_RAČUN1_idx` (`računID` ASC) VISIBLE,
  INDEX `fk_USLUGA_has_RAČUN_USLUGA1_idx` (`uslugaID` ASC) VISIBLE,
  CONSTRAINT `fk_USLUGA_has_RAČUN_USLUGA1`
    FOREIGN KEY (`uslugaID`)
    REFERENCES `LanacHotela`.`USLUGA` (`uslugaID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_USLUGA_has_RAČUN_RAČUN1`
    FOREIGN KEY (`računID`)
    REFERENCES `LanacHotela`.`RACUN` (`racunID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`STAVKA_NA_RACUNU`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`STAVKA_NA_RACUNU` (
  `stavkaID` INT NOT NULL,
  `racunID` INT NOT NULL,
  `cijena` DECIMAL(4,2) NOT NULL,
  `kolicina` INT NOT NULL,
  PRIMARY KEY (`stavkaID`, `racunID`),
  INDEX `fk_STAVKA_has_RAČUN_RAČUN1_idx` (`racunID` ASC) VISIBLE,
  INDEX `fk_STAVKA_has_RAČUN_STAVKA1_idx` (`stavkaID` ASC) VISIBLE,
  CONSTRAINT `fk_STAVKA_has_RAČUN_STAVKA1`
    FOREIGN KEY (`stavkaID`)
    REFERENCES `LanacHotela`.`STAVKA` (`stavkaID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_STAVKA_has_RAČUN_RAČUN1`
    FOREIGN KEY (`racunID`)
    REFERENCES `LanacHotela`.`RACUN` (`racunID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `LanacHotela`.`RACUN_ZA_ARANZMAN`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `LanacHotela`.`RACUN_ZA_ARANZMAN` (
  `racunAranzmanID` INT NOT NULL AUTO_INCREMENT,
  `aranzmanID` INT NOT NULL,
  `cijena` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`racunAranzmanID`),
  INDEX `fk_RACUN_ZA_ARANZMAN_ARANŽMAN1_idx` (`aranzmanID` ASC) VISIBLE,
  CONSTRAINT `fk_RACUN_ZA_ARANZMAN_ARANŽMAN1`
    FOREIGN KEY (`aranzmanID`)
    REFERENCES `LanacHotela`.`ARANZMAN` (`aranzmanID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
