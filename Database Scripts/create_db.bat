ECHO off

sqlcmd -S localhost -E -i sw_planets_db.sql

rem server is localhost

ECHO .
ECHO if no errors appear DB was created
PAUSE