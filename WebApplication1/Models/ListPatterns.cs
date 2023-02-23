namespace API.Models
{
    public static class ListPatterns
    {
        public static string GetSeasons(int[] meses)
            => meses switch
            {
                [12, 1, 2, 3] => "Summer",
                [3, .., 6] => "Fall",
                [6, .., 9] => "Winter",
                [9, .., 12] => "Spring",
                [0 or > 12] => "Thats not a month!",
                [1, ..] => "January, summer ahead",
                [..] => "Random months"
            };

        public static List<string> GetUserInfo()
        {
            var userInfoCollection = new List<string>() 
            {
                "1,Name1,LastName1,Description1",
                "2,Name2,LastName2,Description2",
                "3,Name3,LastName3,Description3"
            };

            var results = new List<string>();

            foreach (var item in userInfoCollection)
            {
                string[] infos = item.Split(',');

                results.Add($"--- User: {infos[0]} ---");

                if (infos is not ["1" or "2", ..])
                {
                    results.Add("Invalid Id");
                    break;
                }

                if (infos is [var id, var firstName, var lastName, ..])
                {
                    results.Add($"Id: {id} | First Name: {firstName} | Last Name: {lastName}");
                }

                if (infos is ["1" or "2", ..])
                {
                    results.Add($"Valid id");
                }

                if (infos is ["1", .., var description])
                {
                    results.Add($"Description: {description}");
                }
            }

            return results;

        }
    }
}
