using System.Collections.Generic;

namespace PoEAA_RowDataGateway
{
    internal class Registry
    {
        private static readonly Registry Instance = new Registry();
        private readonly Dictionary<int, PersonGateway> _personsMap = new Dictionary<int, PersonGateway>();

        private Registry()
        {

        }

        public static void AddPerson(PersonGateway personGateway)
        {
            Instance._personsMap.Add(personGateway.Id, personGateway);
        }

        public static PersonGateway GetPerson(int id)
        {
            if (Instance._personsMap.ContainsKey(id))
            {
                return Instance._personsMap[id];
            }

            return null;
        }
    }
}