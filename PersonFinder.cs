using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace PoEAA_RowDataGateway
{
    class PersonFinder
    {
        private const string FindStatementString = @"
            SELECT id, lastname, firstname, numberOfDependents
            from person
            WHERE id = $id";

        private const string FindResponsibleStatementString = @"
            SELECT id, lastname, firstname, numberOfDependents
            from person
            WHERE numberOfDependents > 0";

        public PersonGateway Find(int id)
        {
            PersonGateway result = Registry.GetPerson(id);
            if (result != null)
            {
                return result;
            }

            try
            {
                using var conn = DbManager.CreateConnection();
                conn.Open();
                using IDbCommand comm = new SQLiteCommand(FindStatementString, conn);
                comm.Parameters.Add(new SQLiteParameter("$id", id));
                using IDataReader reader = comm.ExecuteReader();
                reader.Read();
                result = PersonGateway.Load(reader);
                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public List<PersonGateway> FindResponsibles()
        {
            List<PersonGateway> result = new List<PersonGateway>();
            try
            {
                using var conn = DbManager.CreateConnection();
                conn.Open();
                using IDbCommand comm = new SQLiteCommand(FindResponsibleStatementString, conn);
                using IDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(PersonGateway.Load(reader));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
