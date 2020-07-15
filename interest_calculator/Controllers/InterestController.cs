using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace interest_calculator.Controllers
{
    [Route("api/interest")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        public class Interest
        {
            public int id { get; set; }
            public float principle { get; set; }
            public double interest { get; set; }
        }

        public class Calculation
        {
            public int calId { get; set; }
            public double annInt { get; set; }
            public double monInt { get; set; }
        }

        List<Interest> int_lst = new List<Interest>()
        {
            new Interest(){id=1, principle=10000, interest=2.5},
            new Interest(){id=2, principle=30000, interest=4.0},
            new Interest(){id=3, principle=100000, interest=2.2},
            new Interest(){id=4, principle=55000, interest=5.5},
            new Interest(){id=5, principle=78000, interest=3.6}
        };

        List<Calculation> cal_lst = new List<Calculation>();

        /// <summary>Calculate interest rate (annual/monthly) for a given principle and interest rate if avaliable</summary>
        /// <param principle="principle" interest="interest"></param>
        /// <returns>Interest object id and adds interest calculation object to Calculations list</returns>
        [HttpGet("createInterestCalculation/{principle}/{interest}")]
        public IActionResult GetInterest(float principle, double interest)
        {
            int identity = GetPrincipleIdentity(principle, interest);
            if (identity == 0)
            {
                return new NotFoundResult();
            }
            else
            {
                var al = (interest / 100) * principle;
                var ml = ((interest / 100) / 12) * principle;
                cal_lst.Add(new Calculation() { calId = identity, annInt = al, monInt = ml });
                return new OkObjectResult(identity);
            }
        }

        /// <summary>Calculate interest rate for a given principle and interest rate if avaliable</summary>
        /// <param principle="principle" interest="interest"></param>
        /// <returns>Calculations of the interest annual and monthly with the Interese object id</returns>
        [HttpGet("InterestCalculation/{principle}/{interest}")]
        public IActionResult GetInterestCalcObject(float principle, double interest)
        {
            var identity = GetInterestObject(principle, interest);
            if (identity.id == 0)
            {
                return new NotFoundResult();
            }
            else
            {
                var al = (interest/100) * principle;
                var ml = ((interest/100) / 12) * principle;
                cal_lst.Add(new Calculation() { calId = identity.id, annInt = al, monInt = ml });
                return new OkObjectResult(cal_lst.Where(o => o.calId == identity.id));
            }
        }

        /// <summary>Obtain the object id for the given principle and interest</summary>
        /// <param principle="principle" interest="interest"></param>
        /// <returns>the Id if present in the list</returns>
        private int GetPrincipleIdentity(float principle, double interest)
        {
            var res = int_lst.Where(obj => obj.principle == principle && obj.interest == interest).FirstOrDefault();
            return res.id;
        }

        /// <summary>Obtain the object for the given principle and interest</summary>
        /// <param principle="principle" interest="interest"></param>
        /// <returns>the object of calculations same as the Interest object id if present in the list</returns>
        private Interest GetInterestObject(float principle, double interest)
        {
            var res = int_lst.Where(obj => obj.principle == principle && obj.interest == interest).FirstOrDefault();
            //var result = cal_lst.Where(o => o.calId == res.id).FirstOrDefault();
            return res;
        }
    }
}
