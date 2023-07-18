using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using WebApi.Data;

namespace WebApi.Controllers
{
    //REST (Representational State Transfer)
    //공식, 표준 스펙은 아니다.
    //원래 있던 HTTP통신에서 기능을 '재사용'하여 데이터 송수신 규칙을 만든 것


    //CRUD
    // www.naver.vom -> 현대백화점 지하 1층
    // www.naver.com/sports -> 현대백화점 지하 1층 일식당 XXX
    // verb(GET POST PUT .....)


    //Create
    //POST/api/ranking
    // -- 아이템 생성 요청(Body에 실제 정보)

    //Read
    //GET/api/ranking -> 모든 아이템을 주세요
    //GET/api/ranking/1 => 아이디가 1번인 아이템을 주세요


    //Update
    //PUT/api/ranking(PUT 보안 문제로 웹에서 활용X)
    //아이템 갱신 요청(Body에 실제 정보)


    //Delete
    //DELETE/api/ranking/1(DELETE 보안 문제로 웹에서 활용X)
    //아이디가 1번인 아이템을 삭제해주세요.

    //ApiController 특징
    //그냥 C# 객체를 반환해도 된다.
    //null을 반환하면 -> 클라이언트에 204 Response (No Content)
    // string -> text/plain
    //나머지(int, bool) -> application/json


    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        ApplicationDbContext _context;

        public RankingController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Create
        [HttpPost]
        public GameResult AddGameResult([FromBody]GameResult gameResult)
        {
            _context.GameResults.Add(gameResult);
            _context.SaveChanges();

            return gameResult;
        }

        //Read
        [HttpGet]
        public List<GameResult> GetGameResults()
        {
            List<GameResult> results = _context.GameResults
                            .OrderByDescending(item => item.Score)
                            .ToList();

            return results;
         }

        [HttpGet("{id}")]
        public GameResult GetGameResult(int id)
        {
            GameResult result = _context.GameResults
                            .Where(item => item.Id == id)
                            .FirstOrDefault();

            return result;
        }

        //Update
        [HttpPut]
        public bool UpdateGameResult([FromBody] GameResult gameResult)
        {
            var findResult = _context.GameResults
                        .Where(x => x.Id == gameResult.Id)
                        .FirstOrDefault();

            if (findResult == null)
                return false;

            findResult.UserName = gameResult.UserName;
            findResult.Score = gameResult.Score;
            _context.SaveChanges();

            return true;
        }

        //Delete
        [HttpDelete("{id}")]
        public bool DeleteGameResult(int id)
        {
            var findResult = _context.GameResults
                        .Where(x => x.Id == id)
                        .FirstOrDefault();

            if (findResult == null)
                return false;

            _context.GameResults.Remove(findResult);
            _context.SaveChanges();

            return true;
        }

    }
}
