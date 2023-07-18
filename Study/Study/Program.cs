using System;
using System.Linq;
using System.Security.Cryptography;

namespace Study
{
    public enum ClassType
    {
        Knight,
        Archer,
        Mage
    }

    public class Player
    {
        public ClassType ClassType { get; set; }
        public int Level { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public List<int> Items { get; set; } = new List<int>();
    }
    class Program
    {
        static List<Player> _players = new List<Player>();
        static async Task Main(string[] args)
        {
            Random rand = new Random();

            for(int i = 0; i < 100; i++)
            {
                ClassType type = ClassType.Knight;
                switch(rand.Next(3))
                {
                    case 0:
                        type = ClassType.Knight;
                        break;
                    case 1:
                        type = ClassType.Archer;
                        break;
                    case 2:
                        type = ClassType.Mage;
                        break;
                }

                Player player = new Player()
                {
                    ClassType = type,
                    Level = rand.Next(1, 10),
                    HP = rand.Next(100, 1000),
                    Attack = rand.Next(5, 50)
                };

                for (int j = 0; j < 5; j++)
                    player.Items.Add(rand.Next(1, 101));

                _players.Add(player);
            }

            // Q) 레벨이 50이상인 knight만 추려내서 레벨을 낮음 -> 높음 순서로 정렬
            //일반 버젼
            {
                List<Player> players = GetHighLevelKnight();
                foreach (Player p in players)
                {
                    Console.WriteLine($"{p.Level} {p.HP}");
                }
            }

            //LINQ
            {
                //from(foreach)
                //where(if)
                //orderby(sort 기본적으로 오름차순 ascending/ descending)
                //select(최종 결과 추출 -> 가공해서 추출?) 

                var players =
                    from p in _players
                    where p.ClassType == ClassType.Knight && p.Level >= 50
                    orderby p.Level ascending
                    select p;

                foreach (Player p in players)
                {
                    Console.WriteLine($"{p.Level} {p.HP}");
                }
            }

            //중첩 from
            // ex)모든 아이템 목록을 추출
            {
                var playerItems =  from p in _players
                                   from i in p.Items
                                   where i < 30
                                   select new { p, i };

                var li = playerItems.ToList();
            }

            //grouping
            {
                var playersByLevel = from p in _players
                                     group p by p.Level into g
                                     orderby g.Key
                                     select new { g.Key, Players = g };
            }

            //join(내부 조인)
            //outer join(외부 조인)
            {
                List<int> levels = new List<int>() { 1, 5, 9};

                var playerLevels = from p in _players
                                   join l in levels
                                   on p.Level equals l
                                   select p;
            }

            //LINQ 표준 연산자
            {
                var players =
                    from p in _players
                    where p.ClassType == ClassType.Knight && p.Level >= 50
                    orderby p.Level ascending
                    select p;

                var player2 = _players
                    .Where(p => p.ClassType == ClassType.Knight && p.Level >= 50)
                    .OrderBy(p => p.Level)
                    .Select(p => p);
            }

        }

        static public List<Player> GetHighLevelKnight()
        {
            List<Player> players = new List<Player>();

            foreach(Player player in _players)
            {
                if (player.ClassType != ClassType.Knight)
                    continue;
                if (player.Level < 50)
                    continue;

                players.Add(player);
            }

            players.Sort((lhs, rhs) => { return lhs.Level - rhs.Level; });

            return players;
        }
    }
}