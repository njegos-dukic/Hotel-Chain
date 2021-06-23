USE LanacHotela;
 
-- ------ --
-- HOTELI --
-- ------ --

INSERT INTO HOTEL VALUES (0, 'Courtyard by Marriott', 3, 'Prvog krajiškog korpusa', '33', 'Banja Luka', 78000, 'Bosna i Hercegovina'),
 						 (0, 'Hotel Zepter Palas', 3, 'Kralja Petra I Karađorđevića', '60', 'Banja Luka', 78000, 'Bosna i Hercegovina'),
 						 (0, 'Provence', 4, 'Miloša Obilića', '37', 'Banja Luka', 78000, 'Bosna i Hercegovina'),
                         (0, 'Hotel LoRa', 4, 'Starca Vujadina', '8', 'Beograd', 11080, 'Srbija'),
                         (0, 'Marquise Hotel', 5, 'Mišarska', '6', 'Beograd', 11000, 'Srbija'),
                         (0, 'The Westin Zagreb', 5, 'Krsnjavoga', '1', 'Zagreb', 10000, 'Hrvatska'),
                         (0, 'Hotel Central', 3, 'Kneza Branimira', '3', 'Zagreb', 11080, 'Hrvatska'); 
                          
-- --------------- --
-- KONTAKTI HOTELA --
-- --------------- --

INSERT INTO KONTAKT VALUES (0, 'email', 'courtyard.bl@marriot.com',  1, NULL),
                           (0, 'telefon', '051/510-520',  1, NULL),
                           (0, 'telefon', '051/231-132',  1, NULL),
                           (0, 'email', 'lora-hotel@lorabg.com', 4, NULL),
                           (0, 'email', 'palas@blhotel.ba',  2, NULL),
                           (0, 'email', 'provence@provenceresort.ba', 3, NULL),
                           (0, 'telefon', '223/885-123', 5, NULL),
                           (0, 'email', 'marquies@marqshotels.srb', 5, NULL),
                           (0, 'email', 'westin-reservation@westin.cro', 6, NULL),
                           (0, 'email', 'westin-management@westin.cro', 6, NULL),
                           (0, 'telefon', '989/225-145', 6, NULL),
                           (0, 'telefon', '765/551-125', 7, NULL);

-- ---- --
-- SOBE --
-- ---- --

INSERT INTO SOBA VALUES (0, 1, 01, 2, TRUE, FALSE, 125.50, 1),
						(0, 1, 02, 1, TRUE, FALSE, 105.50, 1),
                        (0, 2, 01, 3, FALSE, FALSE, 145.50, 1),
                        (0, 3, 01, 1, TRUE, TRUE, 225.00, 1),
				
                        (0, 1, 101, 3, TRUE, FALSE, 75.30, 2),
                        (0, 1, 102, 3, TRUE, TRUE, 100.00, 2),
                        (0, 2, 201, 1, TRUE, TRUE, 95.00, 2),
                        
                        (0, 1, 11, 3, TRUE, TRUE, 115.50, 3),
                        (0, 1, 12, 3, FALSE, TRUE, 95.50, 3),
                        
                        (0, 1, 101, 1, FALSE, TRUE, 105.00, 4),
                        (0, 2, 201, 2, TRUE, TRUE, 155.70, 4),
                        (0, 3, 301, 3, FALSE, TRUE, 105.60, 4),
                        
                        (0, 1, 1, 3, FALSE, TRUE, 60.10, 5),
                        
                        (0, 1, 12, 3, FALSE, TRUE, 95.50, 6),
                        (0, 1, 12, 3, FALSE, TRUE, 95.50, 6),
                        
                        (0, 1, 11, 1, FALSE, TRUE, 160.00, 7),
                        (0, 1, 12, 2, TRUE, TRUE, 200.00, 7),
                        (0, 2, 21, 1, TRUE, TRUE, 175.00, 7),
                        (0, 2, 22, 2, TRUE, FALSE, 145.00, 7); 
      
-- ----- --
-- GOSTI --
-- ----- --

INSERT INTO GOST VALUES (0, '1234567890123', 'Gost', 'Prvi'),
						(0, '1512903553125', 'Gost', 'Drugi'),
                        (0, '0993531185913', 'Gost', 'Treci'),
                        (0, '9080951261258', 'Gost', 'Cetvrti'),
                        (0, '7135353521671', 'Gost', 'Peti'),
                        (0, '1295127512600', 'Gost', 'Sesti'),
                        (0, '8216051830151', 'Gost', 'Sedmi'),
                        (0, '1289582106981', 'Gost', 'Osim'),
                        (0, '8210511128825', 'Gost', 'Deveti'); 
                        
-- ---------------- --
-- KONTAKTI GOSTIJU --
-- ---------------- --

INSERT INTO KONTAKT VALUES (0, 'email', 'prvi@gmail.com', NULL, 1),
						   (0, 'email', 'drugi@yahoo.com', NULL, 2),
                           (0, 'telefon', '665/123-512', NULL, 3),
                           (0, 'telefon', '444/987-112', NULL, 4),
                           (0, 'email', 'peti@hotmail.com', NULL, 5),
                           (0, 'email', 'sesti@microsoft.com', NULL, 6),
                           (0, 'telefon', '701/207-124', NULL, 7),
                           (0, 'email', 'osmi@mail.com', NULL, 8),
                           (0, 'telefon', '125/215-999', NULL, 9); 

-- --------- --
-- ARANZMANI --
-- --------- --

INSERT INTO ARANZMAN VALUES (0, '2021-05-12', '2021-05-15', FALSE, TRUE, 1, 1, 1),
							(0, '2020-12-05', '2020-12-17', FALSE, TRUE, 1, 2, 2),
                            (0, '2021-03-03', '2021-03-10', TRUE, FALSE, 1, 3, 4),
                            (0, '2021-01-07', '2021-01-14', FALSE, TRUE, 2, 3, 5),
                            (0, '2022-12-12', '2022-12-15', FALSE, FALSE, 2, 4, 6),
                            (0, '2021-02-01', '2021-02-03', TRUE, FALSE, 2, 4, 6),
                            (0, '2021-01-12', '2021-01-15', FALSE, TRUE, 3, 5, 8),
                            (0, '2021-02-22', '2021-02-23', FALSE, TRUE, 3, 9, 9),
                            (0, '2019-03-11', '2019-03-15', FALSE, TRUE, 4, 6, 10),
                            (0, '2020-11-11', '2020-11-17', FALSE, TRUE, 4, 7, 11),
                            (0, '2021-06-06', '2021-06-15', FALSE, TRUE, 4, 8, 12),
                            (0, '2019-10-10', '2019-10-12', FALSE, TRUE, 5, 1, 13),
                            (0, '2022-03-03', '2022-03-14', FALSE, FALSE, 6, 2, 14),
                            (0, '2020-09-25', '2020-10-01', TRUE, FALSE, 6, 3, 15),
                            (0, '2018-07-13', '2018-08-01', FALSE, TRUE, 7, 4, 16),
                            (0, '2022-08-23', '2022-08-29', FALSE, FALSE, 7, 6, 17),
                            (0, '2022-04-04', '2022-04-06', FALSE, FALSE, 7, 5, 18),
                            (0, '2020-09-21', '2020-09-25', FALSE, TRUE, 7, 9, 19),
                            (0, '2022-09-18', '2022-09-20', FALSE, FALSE, 1, 2, 1),
                            (0, '2022-08-01', '2022-08-03', FALSE, FALSE, 2, 6, 6); 

-- ---------------- --
-- RACUNI ARANZMANA --
-- ---------------- --

CALL KreirajRacuneAranzmana();