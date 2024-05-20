# Airtel-MTN-CBS
# CONFIGUTING THE DATABASE (MYSQL DATABASE )

# INSTALL MYSQL :
sudo apt-get install mysql-server

# Secure MySQL Installation:
sudo mysql_secure_installation

<!-- # Start MySQL
sudo service mysql start

# Verify Installation: 
mysql -u root -p -->

# Log in to MySQL 
sudo mysql -u root

# Set the root password and configure MySQL to use password authentication for the root user:
ALTER USER 'root'@'localhost' IDENTIFIED WITH mysql_native_password BY '*********';

# Flush the privileges to ensure that the changes take effect:
FLUSH PRIVILEGES;

# Exit MySQL:
EXIT;

# log in to MySQL using the new root password:
mysql -u root -p

# Check Users and Authentication Methods:
SELECT user, host, plugin FROM mysql.user WHERE user = 'root';
# Create a Database 
CREATE DATABASE your_database_name;

# Create a New User FOR THE DATABASE:
CREATE USER 'Winnie'@'localhost' IDENTIFIED BY '********';
GRANT ALL PRIVILEGES ON BankingSystem.* TO 'Winnie'@'localhost';
FLUSH PRIVILEGES;



# RELATIONSHIPS
# User Model:

One user can have multiple transactions, defined by the ICollection<Transaction> Transactions property.
ONE TO MANY RELATIONSHIP
# Transaction Model:

Each transaction belongs to one user, defined by the User User property. (ONE TO ONE )
Each transaction has one status, defined by the TransactionStatus Status property.(ONE TO ONE)
Each transaction can have multiple additional infos, defined by the ICollection<AdditionalInfo> AdditionalInfos property.(ONE TO MANY RELATIONSHIP)
# TransactionStatus Model:

Each status can be associated with multiple transactions, defined by the ICollection<Transaction> Transactions property.(ONE TO MANY)
# AdditionalInfo Model:

Each additional info is related to one transaction, defined by the Transaction Transaction property.(ONE TO ONE)

# RUNNING MIGRATIONS 