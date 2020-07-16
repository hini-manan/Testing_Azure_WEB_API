using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace interest_calculator.Controllers
{
    [Route("api/interest")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        // Interest object Class to hold properties needed for calculating interest
        public class Interest
        {
            public int id { get; set; }
            public float principle { get; set; }
            public double interest { get; set; }
        }
        // Calculation onbect Class to calculate the annual and monthly interest for the given principle and interest rate
        public class Calculation
        {
            public int calId { get; set; }
            public double annInt { get; set; }
            public double monInt { get; set; }
        }

        // List holding set of predefined Interest rates and Principles
        List<Interest> int_lst = new List<Interest>()
        {
            new Interest(){id=1, principle=10000, interest=2.5},
            new Interest(){id=2, principle=30000, interest=4.0},
            new Interest(){id=3, principle=100000, interest=2.2},
            new Interest(){id=4, principle=55000, interest=5.5},
            new Interest(){id=5, principle=78000, interest=3.6}
        };
        // List to hold set of calculated annual and monthly interests
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
            var al = (interest / 100) * principle;
            var ml = ((interest / 100) / 12) * principle;
            cal_lst.Add(new Calculation() { calId = identity, annInt = al, monInt = ml });
            return new OkObjectResult(identity);
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
            var al = (interest/100) * principle;
            var ml = ((interest/100) / 12) * principle;
            cal_lst.Add(new Calculation() { calId = identity.id, annInt = al, monInt = ml });
            return new OkObjectResult(cal_lst.Where(o => o.calId == identity.id));
        }

        /// <summary>Update the interest rated for the given principle</summary>
        /// <param principle="principle" interest="interest"></param>
        /// <returns>updated calculations</returns>
        [HttpPut("updateInterest/{principle}/{interest}")]
        public IActionResult updateInterest(float principle, double interest)
        {
            var res = GetInterestObject(principle);
            if (res == null)
            {
                return new NotFoundResult();
            }
            res.interest = interest;
            var al = (res.interest / 100) * principle;
            var ml = ((res.interest / 100) / 12) * principle;
            cal_lst.Add(new Calculation() { calId = res.id, annInt = al, monInt = ml });
            return new OkObjectResult(cal_lst.Where(o => o.calId == res.id));
        }

        /// <summary>Add a new interest and principle object and calculate interest</summary>
        /// <param principle="principle" interest="interest"></param>
        /// <returns>calculated interest of the added interest object</returns>
        [HttpPost("addInterestObj/{principle}/{interest}")]
        public IActionResult addInterest(float principle, double interest)
        {
            int_lst.Add(new Interest() { id=(int_lst.Count+1), principle = principle, interest = interest });
            var res = int_lst[(int_lst.Count - 1)];

            var al = (res.interest / 100) * principle;
            var ml = ((res.interest / 100) / 12) * principle;
            cal_lst.Add(new Calculation() { calId = res.id, annInt = al, monInt = ml });
            return new OkObjectResult(cal_lst);
        }

        /// <summary>delete an interest object record with a given principle</summary>
        /// <param principle="principle"></param>
        /// <returns>the interest object list without the removed object</returns>
        [HttpDelete("deleteObject/{principle}")]
        public IActionResult deleteInterest(float principle)
        {
            var res = GetInterestObject(principle);
            if (res == null)
            {
                return new NotFoundResult();
            }

            int_lst.Remove(res);
            return new OkObjectResult(int_lst);
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
        private Interest GetInterestObject(float principle)
        {
            var res = int_lst.Where(obj => obj.principle == principle).FirstOrDefault();
            //var result = cal_lst.Where(o => o.calId == res.id).FirstOrDefault();
            return res;
        }
    }
}
