using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Galytix.WebApi.Models;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Galytix.WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class gwpController : ControllerBase
    {
        /*
        [AllowAnonymous]
        [HttpGet]
        [Route("ping")]
        public string Ping()
        {
            return "Gopal Jain's Program";
        }
        */

        public List<Data> records = new List<Data>();
        public List<Memoization> memo = new List<Memoization>();
        public gwpController()
        {
            


            // Creating records
            using (var reader = new StreamReader(@"..\..\..\Data\gwpByCountry.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<Data>().ToList();
            }

            // Preprocessing to compute results
            foreach (var record in records)
            {
                double avg = 0;
                // 2008 - 2015
                if (record.Y2008 != null) avg += record.Y2008.Value;
                if (record.Y2009 != null) avg += record.Y2009.Value;
                if (record.Y2010 != null) avg += record.Y2010.Value;
                if (record.Y2011 != null) avg += record.Y2011.Value;
                if (record.Y2012 != null) avg += record.Y2012.Value;
                if (record.Y2013 != null) avg += record.Y2013.Value;
                if (record.Y2014 != null) avg += record.Y2014.Value;
                if (record.Y2015 != null) avg += record.Y2015.Value;
                avg /= 8;
                Memoization curr = new Memoization();
                curr.country = record.country;
                curr.lineOfBusiness = record.lineOfBusiness;
                curr.avg = avg;
                memo.Add(curr);
            }

        }


        [HttpPost]
        [Route("avg")]
        public Dictionary<string, double> Avg(Item item)
        {
            var list = new Dictionary<string, double>();
            // Introduce main algorithm
            foreach (string it in item.Lob)
            {
                foreach (var record in memo)
                {
                    if (record.country == item.Country && record.lineOfBusiness == it)
                    {
                        double temp = (double)record.avg;
                        temp = Math.Round(temp, 1);
                        list.Add(it, temp);
                        break;
                    }
                }
            }


            return list;
        }
    }
}