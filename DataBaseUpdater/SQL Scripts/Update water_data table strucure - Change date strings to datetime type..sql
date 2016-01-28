# Dates were being stored in the database 
# as strings. This was causing numerous issues 
# in the database updater application and needs 
# to be updated so that dates appear as datetime types.

# Created By: John August 
# Date Created: 27-Jan-2016, 10:07PM

# Test conversion method:
#select * from rtidev.water_data limit 100;
#select DATE_FORMAT(str_to_date(measurment_date, '%m/%d/%Y %H:%i'),'%m/%d/%Y %H:%i'), measurment_date AS 'DATE' from rtidev.water_data limit 5;

# Investigate table structure
EXPLAIN water_data;
# Field, 		  Type, 		  Null
#measurment_date, varchar(255),   NO

# Temporarily Re-Name the old "measurment_date" column
ALTER TABLE rtidev.water_data CHANGE measurment_date date_time VARCHAR(255) NOT NULL;

# Add in the new date column
ALTER TABLE rtidev.water_data ADD measurment_date DATETIME NOT NULL;

# Convert all string dates to datetme dates
UPDATE water_data
SET measurment_date = str_to_date(date_time, '%m/%d/%Y %H:%i')
WHERE dataID <> '-1';

# Confirm change
SELECT cond AS CONDUCTIVITY, date_time AS 'measument_date(OLD)', measurment_date FROM rtidev.water_data;

# Delete the temporarily re-named old column
ALTER TABLE rtidev.water_data DROP date_time;

# Reposition the new measurment_date column
ALTER TABLE rtidev.water_data CHANGE COLUMN measurment_date measurment_date VARCHAR(50) AFTER temp;

# DEBUG
SELECT * FROM water_data






