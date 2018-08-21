#!/bin/bash

# Start SQL Server
#/opt/mssql/bin/sqlservr &

# Start the script to create the DB and user
#/usr/config/configure-db.sh
#./configure-db.sh
export STATUS=1
i=0

while [[ $STATUS -ne 0 ]] && [[ $i -lt 30 ]]; do
	i=$i+1
	/opt/mssql-tools/bin/sqlcmd -t 1 -U sa -P $MSSQL_SA_PASSWORD -Q "select 1" >> /dev/null
	STATUS=$?
done

if [ $STATUS -ne 0 ]; then 
	echo "Error: MSSQL SERVER took more than thirty seconds to start up."
	exit 1
fi

echo "======= MSSQL SERVER STARTED ========" | tee -a ./config.log
# Run the setup script to create the DB and the schema in the DB
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $MSSQL_SA_PASSWORD -d master -i setup_db.sql

echo "======= MSSQL CONFIG COMPLETE =======" | tee -a ./config.log

# Call extra command
eval $1