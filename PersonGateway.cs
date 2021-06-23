using System;
using System.Data;
using System.Data.SQLite;

namespace PoEAA_RowDataGateway
{
    class PersonGateway : BaseGateway
    {
        public PersonGateway(int id, string lastName, string firstName, int numberOfDependents)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            NumberOfDependents = numberOfDependents;
        }

        private const string UpdateStatementString =
            @"UPDATE person 
                    SET lastname = $lastname, firstname = $firstname, numberOfDependents = $numberOfDependents
            where id = $id";

        private const string InsertStatementString =
            @"INSERT INTO person 
                    VALUES ($id, $lastname, $firstname, $numberOfDependents)";

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int NumberOfDependents { get; set; }

        public static PersonGateway Load(IDataReader reader)
        {
            object[] resultSet = new object[reader.FieldCount];
            reader.GetValues(resultSet);

            int id = (int) resultSet[0];
            PersonGateway result = Registry.GetPerson(id);
            if (result != null)
            {
                return result;
            }

            string lastName = resultSet[1].ToString();
            string firstName = resultSet[2].ToString();
            int numberOfDependents = (int)resultSet[3];
            result = new PersonGateway(id, lastName, firstName, numberOfDependents);
            Registry.AddPerson(result);
            return result;
        }

        public void Update()
        {
            try
            {
                using var conn = DbManager.CreateConnection();
                conn.Open();
                using IDbCommand comm = new SQLiteCommand(UpdateStatementString, conn);
                comm.Parameters.Add(new SQLiteParameter("$lastname", LastName));
                comm.Parameters.Add(new SQLiteParameter("$firstname", FirstName));
                comm.Parameters.Add(new SQLiteParameter("$numberOfDependents", NumberOfDependents));
                comm.Parameters.Add(new SQLiteParameter("$id", Id));
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public int Insert()
        {
            try
            {
                using var conn = DbManager.CreateConnection();
                conn.Open();
                using IDbCommand comm = new SQLiteCommand(InsertStatementString, conn);
                Id = FindNextDatabaseId();
                comm.Parameters.Add(new SQLiteParameter("$id", Id));
                comm.Parameters.Add(new SQLiteParameter("$lastname", LastName));
                comm.Parameters.Add(new SQLiteParameter("$firstname", FirstName));
                comm.Parameters.Add(new SQLiteParameter("$numberOfDependents", NumberOfDependents));
                comm.ExecuteNonQuery();
                Registry.AddPerson(this);

                return Id;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private int FindNextDatabaseId()
        {
            string sql = "SELECT max(id) as curId from person";
            using var conn = DbManager.CreateConnection();
            conn.Open();
            using IDbCommand comm = new SQLiteCommand(sql, conn);
            using IDataReader reader = comm.ExecuteReader();
            bool hasResult = reader.Read();
            if (hasResult)
            {
                return ((int)((long)reader["curId"] + 1));
            }
            else
            {
                return 1;
            }
        }
    }
}
