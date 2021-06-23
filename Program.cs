using System;
using System.Collections.Generic;

namespace PoEAA_RowDataGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeData();

            Console.WriteLine("Get responsible persons");
            PersonFinder finder = new PersonFinder();
            var people = finder.FindResponsibles();
            PrintPersonGateway(people);

            Console.WriteLine("Insert a new person");
            new PersonGateway(0, "Rose", "Jackson", 60).Insert();
            people = finder.FindResponsibles();
            PrintPersonGateway(people);

            Console.WriteLine("Update a person's first name");
            var firstPerson = finder.Find(1);
            firstPerson.FirstName = "Jack";
            firstPerson.Update();

            Console.WriteLine("Update a person's number of dependents");
            var secondPerson = finder.Find(2);
            secondPerson.NumberOfDependents = 0;
            secondPerson.Update();

            Console.WriteLine("Get responsible persons again");
            people = finder.FindResponsibles();
            PrintPersonGateway(people);

        }

        private static void PrintPersonGateway(IEnumerable<PersonGateway> people)
        {
            foreach (var person in people)
            {
                Console.WriteLine($"ID: {person.Id}, last name: {person.LastName}, first name: {person.FirstName}, number of dependents: {person.NumberOfDependents}");
            }
        }

        private static void InitializeData()
        {
            using (var connection = DbManager.CreateConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        @"
                        DROP TABLE IF EXISTS person;
                    ";
                    command.ExecuteNonQuery();


                    command.CommandText =
                        @"
                        CREATE TABLE person (Id int primary key, lastname TEXT, firstname TEXT, numberOfDependents int);
                    ";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        @"
                       
                    INSERT INTO person
                        VALUES (1, 'Sean', 'Reid', 5);

                    INSERT INTO person
                        VALUES (2, 'Madeleine', 'Lyman', 13);

                    INSERT INTO person
                        VALUES (3, 'Oliver', 'Wright', 66);
                    ";
                    command.ExecuteNonQuery();
                }

            }
        }
    }
}
